using System.Globalization;
using System.Net;
using NLog;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Util;

namespace Nummi.Core.External.Binance; 

public class BinanceClient {

    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private NummiHttpClient Client { get; } = new(new HttpClient(), "https://api.binance.us/api/v3");
    private DateTime BlockedUntil { get; set; } = DateTime.MinValue;
    private int MinuteRequestWeightLimit { get; set; }

    private DateTime LastMinuteRequestAt { get; set; } = DateTime.MinValue;
    private int UsedMinuteRequestWeightLimit { get; set; }

    public BinanceClient() {
        try {
            Client
                .OnStatusCode(HttpStatusCode.TooManyRequests, HandleTooManyRequests)
                .OnStatusCode((HttpStatusCode) 419, HandleIpBanned)
                .LogHeaders("x-mbx-used-weight", "x-mbx-used-weight-1m", "retry-after");

            MinuteRequestWeightLimit = 1000000;
            var exchangeInfo = GetExchangeInfo();
            MinuteRequestWeightLimit = exchangeInfo.RateLimits
                .Where(v => v is { RateLimitType: "REQUEST_WEIGHT", Interval: "MINUTE", IntervalNum: 1 })
                .Select(v => (int) v.Limit)
                .First();
        }
        catch (Exception e) {
            throw e;
        }
    }

    public IEnumerable<Price> GetSpotPrice(ISet<string> symbols) {
        const int weight = 2;
        var now = DateTime.UtcNow;
        
        CheckIfWeightLimitShouldReset(now);
        AssertNotBlocked(now);
        AssertWillNotSurpassWeightLimit(weight);

        var result = Client.Get("/ticker/price")
            .Parameter("symbols", symbols)
            .Execute()
            .ReadFirstHeader("x-mbx-used-weight-1m", out var usedWeight, int.Parse)
            .ReadJson<IList<BinancePrice>>()
            .Select(v => v.ToHistoricalPrice());

        LastMinuteRequestAt = now;
        UsedMinuteRequestWeightLimit = usedWeight;
        
        return result;
    }

    private void CheckIfWeightLimitShouldReset(DateTime now) {
        if (LastMinuteRequestAt + TimeSpan.FromSeconds(1) <= now) {
            UsedMinuteRequestWeightLimit = 0;
        }
    }
    
    public IDictionary<string, MinuteBar> GetMinuteKlines(ISet<string> symbols) {
        const int weightPerCall = 1;
        int totalWeight = symbols.Count * weightPerCall;
        var now = DateTime.UtcNow;
        
        CheckIfWeightLimitShouldReset(now);
        AssertNotBlocked(now);
        AssertWillNotSurpassWeightLimit(totalWeight);

        var response = new Dictionary<string, MinuteBar>(symbols.Count);
        var startTime = ((DateTimeOffset)DateTime.UtcNow.AddMinutes(-1));

        foreach (var symbol in symbols) {
            var kline = Client.Get("/klines")
                .Parameter("symbol", symbol)
                .Parameter("limit", "1")
                .Parameter("interval", "1m")
                .Parameter("startTime", startTime.ToUnixTimeMilliseconds().ToString())  // Actually does +1 minutes
                .Execute()
                .ReadFirstHeader("x-mbx-used-weight-1m", out var usedWeight, int.Parse)
                .ReadJsonElement();
        
            LastMinuteRequestAt = now;
            UsedMinuteRequestWeightLimit = usedWeight;

            var klineIter = kline.EnumerateArray();
            while (klineIter.MoveNext()) {
                var candlestickValues = klineIter.Current.EnumerateArray().ToList();
                response[symbol] = new MinuteBar(
                    symbol: symbol,
                    openTimeEpoch: candlestickValues[0].GetInt64(),
                    closeTimeEpoch: candlestickValues[6].GetInt64(),
                    open: decimal.Parse(candlestickValues[1].GetString()!),
                    high: decimal.Parse(candlestickValues[2].GetString()!),
                    low: decimal.Parse(candlestickValues[3].GetString()!),
                    close: decimal.Parse(candlestickValues[4].GetString()!),
                    volume: decimal.Parse(candlestickValues[5].GetString()!)
                );
                Log.Info($"{startTime.ToString(CultureInfo.InvariantCulture).Blue()}");
                Log.Info($"{startTime.ToUnixTimeMilliseconds().ToString().Blue()}");
                Log.Info($"{response[symbol].OpenTimeUtc.ToString(CultureInfo.InvariantCulture).Yellow()}");
                Log.Info($"{response[symbol].OpenTimeEpoch.ToString().Yellow()}");
                Log.Info($"{response[symbol].CloseTimeUtc.ToString(CultureInfo.InvariantCulture).Yellow()}");
                Log.Info($"{response[symbol].Close.ToString(CultureInfo.InvariantCulture).Green()}");
            }
        }

        return response;
    }

    private void AssertNotBlocked(DateTime now) {
        if (BlockedUntil > now) {
            throw new Exception();
        }
    }
    
    private void AssertWillNotSurpassWeightLimit(int callWeight) {
        if (UsedMinuteRequestWeightLimit + callWeight > MinuteRequestWeightLimit) {
            throw new Exception();
        }
    }
    
    public HistoricalBars GetMinuteKlines(ISet<string> symbols, DateTime start) {
        const int apiLimit = 1000;
        const int weightPerCall = 1;
        int weightPerAllSymbolsCall = weightPerCall * symbols.Count;
        var now = DateTime.UtcNow;
        start = start.AddMinutes(-1);

        CheckIfWeightLimitShouldReset(now);
        AssertNotBlocked(now);
        AssertWillNotSurpassWeightLimit(weightPerAllSymbolsCall);

        int bars = (now - start).Minutes + 1;
        
        if (bars > apiLimit) {
            bars = apiLimit;
        }
        
        var result = new Dictionary<string, IEnumerable<MinuteBar>>();
        foreach (var symbol in symbols) {
            result[symbol] = GetMinuteKlines(symbol, start, bars);
        }

        var timeSpan = TimeSpan.FromMinutes(bars);
        return new HistoricalBars(
            timeSpan: timeSpan,
            endTime: start + timeSpan,
            bars: result
        );
    }
    
    private IEnumerable<MinuteBar> GetMinuteKlines(string symbol, DateTime startTime, int limit) {
        var kline = Client.Get("/klines")
            .Parameter("symbol", symbol)
            .Parameter("limit", limit.ToString())
            .Parameter("interval", "1m")
            .Parameter("startTime", ((DateTimeOffset)startTime).ToUnixTimeMilliseconds().ToString())
            .Execute()
            .ReadJsonElement();

        // Log.Info($"{startTime.AddMinutes(1).ToString(CultureInfo.InvariantCulture).Blue()}");
        // Log.Info($"{((DateTimeOffset)startTime).ToUnixTimeMilliseconds().ToString().Purple()}");
        var result = new List<MinuteBar>();
        var klineIter = kline.EnumerateArray();
        while (klineIter.MoveNext()) {
            var candlestickValues = klineIter.Current.EnumerateArray().ToList();
            var bar = new MinuteBar(
                symbol: symbol,
                openTimeEpoch: candlestickValues[0].GetInt64(),
                closeTimeEpoch: candlestickValues[6].GetInt64(),
                open: decimal.Parse(candlestickValues[1].GetString()!),
                high: decimal.Parse(candlestickValues[2].GetString()!),
                low: decimal.Parse(candlestickValues[3].GetString()!),
                close: decimal.Parse(candlestickValues[4].GetString()!),
                volume: decimal.Parse(candlestickValues[5].GetString()!)
            );
            result.Add(bar);
            // Log.Info($"{bar.OpenTimeUtc.ToString(CultureInfo.InvariantCulture).Yellow()}");
            // Log.Info($"{bar.CloseTimeUtc.ToString(CultureInfo.InvariantCulture).Yellow()}");
            // Log.Info($"{bar.Close.ToString(CultureInfo.InvariantCulture).Green()}");
        }
        // Log.Info($"Expected {limit.ToString(CultureInfo.InvariantCulture).Yellow()} bars");
        // Log.Info($"Received {result.Count.ToString(CultureInfo.InvariantCulture).Green()} bars");
        return result;
    }

    public ExchangeInfo GetExchangeInfo() {
        const int weight = 10;
        var now = DateTime.UtcNow;

        CheckIfWeightLimitShouldReset(now);
        AssertNotBlocked(now);
        
        if (UsedMinuteRequestWeightLimit + weight > MinuteRequestWeightLimit) {
            throw new Exception();
        }

        var result = Client.Get("/exchangeInfo")
            .Execute()
            .ReadFirstHeader("x-mbx-used-weight-1m", out var usedWeight, int.Parse)
            .ReadJson<ExchangeInfo>();
        
        LastMinuteRequestAt = now;
        UsedMinuteRequestWeightLimit = usedWeight;

        return result;
    }

    private void HandleTooManyRequests(HttpResponseReader res) {
        res.ReadFirstHeader(
            key: "retry-after",
            value: out TimeSpan retryAfter,
            orElse: TimeSpan.FromSeconds(60),
            mapper: v => TimeSpan.FromSeconds(int.Parse(v))
        );
        Log.Info($"{"Warning!!".Yellow()} - Binance has limited us. Will retry after {retryAfter.ToString().Blue()} seconds");
        BlockedUntil = DateTime.UtcNow + retryAfter;
    }

    private void HandleIpBanned(HttpResponseReader res) {
        res.ReadFirstHeader(
            key: "retry-after",
            value: out TimeSpan retryAfter,
            orElse: TimeSpan.FromSeconds(60),
            mapper: v => TimeSpan.FromSeconds(int.Parse(v))
        );
        Log.Info($"{"Error!!".Red()} - Binance has banned us. Will retry after {retryAfter.ToString().Blue()} seconds");
        BlockedUntil = DateTime.UtcNow + retryAfter;
    }
}