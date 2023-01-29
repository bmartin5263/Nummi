using NLog;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Exceptions;

namespace Nummi.Core.External.Binance; 

public class BinanceClientAdapter {
    
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private IBinanceClient Client { get; }
    private bool Initialized { get; set; }
    private int WeightLimit { get; set; }
    private DateTime LastRequestAt { get; set; } = DateTime.MinValue;
    private int UsedWeight { get; set; }

    protected BinanceClientAdapter() {
        Client = null!;
    }

    public BinanceClientAdapter(IBinanceClient client) {
        Client = client;
    }

    public BinanceClientAdapter(IBinanceClient client, ExchangeInfo exchangeInfo) {
        Client = client;
        Initialize(exchangeInfo);
    }

    public ExchangeInfo GetExchangeInfo() {
        var response = Client.GetExchangeInfo();
        UsedWeight = response.UsedWeight1M;
        return response.Content;
    }

    public virtual IDictionary<string, Bar> GetBar(ISet<string> symbols, DateTime time, Period period) {
        return GetBars(symbols, new DateRange(time, time), period)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value[0]);
    }

    public virtual IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period) {
        var dict = symbols.ToDictionary(s => s, _ => dateRange);
        return GetBars(dict, period);
    }

    public virtual IDictionary<string, List<Bar>> GetBars(IDictionary<string, DateRange> symbols, Period period) {
        DateTime now = DateTime.UtcNow;
        CheckIfWeightLimitShouldReset(now);
        
        if (!Initialized) {
            Initialize();
        }

        var symbolsWithCalls = symbols
            .ToDictionary(kvp => kvp.Key, kvp => period.CalculateBarCalls(kvp.Value, Client.GetKlinesMaxLimit));
        var totalCalls = symbolsWithCalls.Values.Sum(v => v.CallCount());
        var totalWeight = Client.GetKlinesWeight * totalCalls;

        foreach (var entry in symbolsWithCalls) {
            Log.Info($"{entry.Key}: {entry.Value}");
        }

        var result = new Dictionary<string, List<Bar>>();
        foreach (var (symbol, callDetails) in symbolsWithCalls) {
            var allBars = new List<Bar>();

            int remainder = (int) callDetails.Remainder;
            DateTime runningStartTime = callDetails.DateRange.Start;
            for (int i = 0; i < callDetails.Chunks; ++i) {
                var bars = GetKlines(now, symbol, runningStartTime, callDetails.DateRange.End, period, Client.GetKlinesMaxLimit);
                if (bars.Count != Client.GetKlinesMaxLimit) {
                    throw new InvalidStateException($"Expected {Client.GetKlinesMaxLimit} bars, instead got {bars.Count}");
                }
                allBars.AddRange(bars);
                runningStartTime += period.Time * Client.GetKlinesMaxLimit;
            }

            var remainingBars = GetKlines(now, symbol, runningStartTime, callDetails.DateRange.End, period, remainder);
            if (remainingBars.Count != remainder) {
                throw new InvalidStateException($"Expected {remainder} bars, instead got {remainingBars.Count}");
            }
            allBars.AddRange(remainingBars);

            allBars.Sort((x, y) => x.OpenTimeUnixMs.CompareTo(y.OpenTimeUnixMs));
            result[symbol] = allBars;
        }
        
        return result;
    }

    private IList<Bar> GetKlines(DateTime now, string symbol, DateTime startTime, DateTime endTime, Period period, int limit) {
        if (UsedWeight == 1000) {
            WaitUntilLimitResets(now);
        }
        
        var response = Client.GetKlines(symbol, startTime, endTime, period, limit);
        UsedWeight += 1;
        
        if (UsedWeight != response.UsedWeight1M && !CheckIfWeightLimitShouldReset(now)) {
            throw new InvalidStateException($"Weight limit not same {UsedWeight}, {response.UsedWeight1M}");
        }
        if (UsedWeight > 1000) {
            throw new InvalidStateException("Weight limit exceeded unexpectedly. Must investigate");
        }

        LastRequestAt = now;
        return response.Content;
    }

    private void Initialize() {
        var exchangeInfo = GetExchangeInfo();
        Initialize(exchangeInfo);
    }

    private void Initialize(ExchangeInfo exchangeInfo) {
        WeightLimit = exchangeInfo.RateLimits
            .Where(v => v is { RateLimitType: "REQUEST_WEIGHT", Interval: "MINUTE", IntervalNum: 1 })
            .Select(v => v.Limit ?? 1000)
            .First();
        Initialized = true;
    }

    private bool CheckIfWeightLimitShouldReset(DateTime now) {
        if (LastRequestAt.Minute != now.Minute || LastRequestAt + TimeSpan.FromMinutes(1) <= now) {
            UsedWeight = 0;
            return true;
        }

        return false;
    }

    private void WaitUntilLimitResets(DateTime now) {
        var waitTime = TimeSpan.FromMinutes(1);
        Client.Wait(waitTime);
        UsedWeight = 0;
    }
    
}