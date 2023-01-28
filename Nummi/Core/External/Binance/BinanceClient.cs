using System.Net;
using System.Text.Json;
using NLog;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Util;

namespace Nummi.Core.External.Binance;

public class BinanceClient : IBinanceClient {

    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private NummiHttpClient Client { get; } = new(new HttpClient(), "https://api.binance.us/api/v3");
    private DateTime BlockedUntil { get; set; } = DateTime.MinValue;
    private int MinuteRequestWeightLimit { get; set; }

    private DateTime LastMinuteRequestAt { get; set; } = DateTime.MinValue;
    private int UsedMinuteRequestWeightLimit { get; set; }

    public BinanceClient() {
        Log.Info("New Binance Client");
        Client
            .OnStatusCode(HttpStatusCode.TooManyRequests, HandleTooManyRequests)
            .OnStatusCode((HttpStatusCode) 419, HandleIpBanned)
            .LogHeaders("x-mbx-used-weight", "x-mbx-used-weight-1m", "retry-after");

        MinuteRequestWeightLimit = 1000000;
        var exchangeInfo = GetExchangeInfo();
        MinuteRequestWeightLimit = exchangeInfo.RateLimits
            .Where(v => v is { RateLimitType: "REQUEST_WEIGHT", Interval: "MINUTE", IntervalNum: 1 })
            .Select(v => v.Limit ?? 1000)
            .First();
    }

    public IDictionary<string, Bar> GetBar(ISet<string> symbols, DateTime time, Period period) {
        return GetBars(symbols, new DateRange(time, time), period)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value[0]);
    }

    public IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period) {
        var dict = symbols.ToDictionary(s => s, _ => dateRange);
        return GetBars(dict, period);
    }

    public IDictionary<string, List<Bar>> GetBars(IDictionary<string, DateRange> symbols, Period period) {
        const int WeightPerCall = 1;
        const double BarLimitPerCall = 1000;

        var symbolsWithCalls = symbols
            .ToDictionary(kvp => kvp.Key, kvp => period.CalculateBarCalls(kvp.Value, BarLimitPerCall));
        var totalCalls = symbolsWithCalls.Values.Sum(v => v.CallCount());
        var totalWeight = WeightPerCall * totalCalls;

        foreach (var entry in symbolsWithCalls) {
            Log.Info($"{entry.Key}: {entry.Value}");
        }

        DateTime now = DateTime.UtcNow;
        CheckIfWeightLimitShouldReset(now);
        AssertNotBlocked(now);
        AssertWillNotSurpassWeightLimit(totalWeight);

        var result = new Dictionary<string, List<Bar>>();
        foreach (var (symbol, callDetails) in symbolsWithCalls) {
            var allBars = new List<Bar>();
            
            DateTime runningStartTime = callDetails.DateRange.Start;
            for (int i = 0; i < callDetails.Chunks; ++i) {
                var bars = GetBars(symbol, runningStartTime, callDetails.DateRange.End, period, (int)BarLimitPerCall);
                allBars.AddRange(bars);
                runningStartTime += period.Time * BarLimitPerCall;
            }

            if (callDetails.Remainder > 0) {
                var bars = GetBars(symbol, runningStartTime, callDetails.DateRange.End, period, (int)callDetails.Remainder);
                allBars.AddRange(bars);
            }

            allBars.Sort((x, y) => x.OpenTimeUnixMs.CompareTo(y.OpenTimeUnixMs));
            result[symbol] = allBars;
        }
        
        return result;
    }

    private IList<Bar> GetBars(string symbol, DateTime startTime, DateTime endTime, Period period, int limit) {
        Log.Info($"GetBars(symbol={symbol}, startTime={startTime.ToLocalTime()}, endTime={endTime.ToLocalTime()}, limit={limit})");
        var klines = Client.Get("/klines")
            .Parameter("symbol", symbol)
            .Parameter("startTime", startTime.ToUnixTimeMs().ToString())
            .Parameter("endTime", endTime.ToUnixTimeMs().ToString())
            .Parameter("interval", period.IntervalParam)
            .Parameter("limit", limit.ToString())
            .Execute()
            .ReadJsonElement();

        // Log.Info($"{startTime.ToString(CultureInfo.InvariantCulture).Blue()}");
        // Log.Info($"{((DateTimeOffset)startTime).ToUnixTimeMilliseconds().ToString().Purple()}");
        var result = new List<Bar>();
        var klineIter = klines.EnumerateArray();
        while (klineIter.MoveNext()) {
            var klineValues = klineIter.Current.EnumerateArray().ToList();
            if (TryParseBarResponse(symbol, klineValues, period, out var minuteBar)) {
                result.Add(minuteBar!);
            }
            // Log.Info($"{bar.OpenTimeUtc.ToString(CultureInfo.InvariantCulture).Yellow()}");
        }
        // Log.Info($"Expected {limit.ToString(CultureInfo.InvariantCulture).Yellow()} klines");
        // Log.Info($"Received {result.Count.ToString(CultureInfo.InvariantCulture).Green()} klines");
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
    
    private static bool TryParseBarResponse(string symbol, IList<JsonElement> jsonArray, Period period, out Bar? result) {
        try {
            result = new Bar(
                symbol: symbol,
                openTimeUnixMs: jsonArray[0].GetInt64(),
                periodMs: period.UnixMs,
                open: decimal.Parse(jsonArray[1].GetString()!),
                high: decimal.Parse(jsonArray[2].GetString()!),
                low: decimal.Parse(jsonArray[3].GetString()!),
                close: decimal.Parse(jsonArray[4].GetString()!),
                volume: decimal.Parse(jsonArray[5].GetString()!)
            );
            return true;
        }
        catch (Exception e) {
            result = null;
            Log.Error($"Failed to Parse {"Bar".Green()} Values {jsonArray}", e);
            return false;
        }
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

    private void CheckIfWeightLimitShouldReset(DateTime now) {
        if (LastMinuteRequestAt + TimeSpan.FromSeconds(1) <= now) {
            UsedMinuteRequestWeightLimit = 0;
        }
    }
}