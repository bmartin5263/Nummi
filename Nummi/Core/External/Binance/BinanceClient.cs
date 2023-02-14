using System.Net;
using System.Text.Json;
using NLog;
using Nummi.Core.Domain.New;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.External.Binance;

public class BinanceClient : IBinanceClient {
    
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private NummiHttpClient Client { get; }
    private DateTimeOffset BlockedUntil { get; set; }
    public int GetKlinesWeight => 1;
    public int GetKlinesMaxLimit => 1000;
    
    public BinanceClient() {
        Log.Debug("New Binance Client");
        BlockedUntil = DateTimeOffset.MinValue;
        Client = new NummiHttpClient(new HttpClient(), "https://api.binance.us/api/v3")
            .OnStatusCode(HttpStatusCode.TooManyRequests, HandleTooManyRequests)
            .OnStatusCode((HttpStatusCode) 419, HandleIpBanned)
            .LogHeaders("x-mbx-used-weight", "x-mbx-used-weight-1m", "retry-after");
    }

    public BinanceResponse<IList<Bar>> GetKlines(string symbol, DateTimeOffset startTime, DateTimeOffset endTime, Period period, int limit) {
        Log.Info($"GetBars(symbol={symbol}, startTime={startTime.ToLocalTime()}, endTime={endTime.ToLocalTime()}, limit={limit})");

        if (DateTimeOffset.UtcNow < BlockedUntil) {
            throw new InvalidStateException($"Binance has blocked us until {BlockedUntil}");
        }

        if (limit is < 0 or > 1000) {
            throw new InvalidUserArgumentException("Limit must be between 0 and 1000");
        }

        var response = Client.Get("/klines")
            .Parameter("symbol", symbol)
            .Parameter("startTime", startTime.Truncate(period.Time).ToUnixTimeMilliseconds().ToString())
            .Parameter("endTime", endTime.Truncate(period.Time).ToUnixTimeMilliseconds().ToString())
            .Parameter("interval", period.IntervalParam)
            .Parameter("limit", limit.ToString())
            .Execute()
            .ReadFirstHeader("x-mbx-used-weight-1m", out var usedWeight, int.Parse)
            .ReadFirstHeader("retry-after", out var retryAfter)
            .ReadJsonElement(out var klines);
        
        var result = new List<Bar>();
        var klineIter = klines.EnumerateArray();
        while (klineIter.MoveNext()) {
            var klineValues = klineIter.Current.EnumerateArray().ToList();
            if (TryParseBarResponse(symbol, klineValues, period, out var minuteBar)) {
                result.Add(minuteBar!);
            }
        }

        return new BinanceResponse<IList<Bar>>(
            statusCode: response.StatusCode,
            content: result,
            usedWeight1M: usedWeight,
            retryAfter: retryAfter == null ? null : int.Parse(retryAfter)
        );
    }
    
    public BinanceResponse<ExchangeInfo> GetExchangeInfo() {
        Log.Debug($"GetExchangeInfo()");
        
        if (DateTime.UtcNow < BlockedUntil) {
            throw new InvalidStateException("Blocked");
        }

        var response = Client.Get("/exchangeInfo")
            .Execute()
            .ReadFirstHeader("x-mbx-used-weight-1m", out var usedWeight, int.Parse)
            .ReadFirstHeader("retry-after", out var retryAfter)
            .ReadJson<ExchangeInfo>(out var result);

        return new BinanceResponse<ExchangeInfo>(
            statusCode: response.StatusCode,
            content: result,
            usedWeight1M: usedWeight,
            retryAfter: retryAfter == null ? null : int.Parse(retryAfter)
        );
    }

    public void Wait(TimeSpan time) {
        Task.Delay(time).Wait();
    }

    private static bool TryParseBarResponse(string symbol, IList<JsonElement> jsonArray, Period period, out Bar? result) {
        try {
            result = new Bar(
                symbol: symbol,
                openTime: jsonArray[0].GetInt64().ToUtcDateTime(),
                period: period.Time,
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
            Log.Error($"Failed to Parse {"Bar".Green()} Values {jsonArray}: {e}");
            return false;
        }
    }
    
    private void HandleTooManyRequests(HttpResponse res) {
        res.ReadFirstHeader(
            key: "retry-after",
            value: out TimeSpan retryAfter,
            orElse: TimeSpan.FromSeconds(60),
            mapper: v => TimeSpan.FromSeconds(int.Parse(v))
        );
        Log.Info($"{"Warning!!".Yellow()} - Binance has limited us. Will retry after {retryAfter.ToString().Blue()} seconds");
        BlockedUntil = DateTime.UtcNow + retryAfter;
    }

    private void HandleIpBanned(HttpResponse res) {
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