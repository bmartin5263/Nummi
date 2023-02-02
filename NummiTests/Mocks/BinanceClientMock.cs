using System.Net;
using NLog;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Exceptions;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace NummiTests.Mocks;

public class BinanceClientMock : IBinanceClient {
    
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    
    public int UsedWeight { get; private set; }
    public int WeightLimit { get; private set; } = 1000;
    public DateTime LastRequestAt { get; private set; } = DateTime.MinValue;
    public DateTime Now { get; set; } = DateTime.UtcNow;
    
    public int GetKlinesWeight => 1;
    public int GetKlinesMaxLimit => 1000;

    public ISet<WaitCall> WaitCalls { get; } = new HashSet<WaitCall>();
    public ISet<GetKlinesCall> GetKlinesCalls { get; } = new HashSet<GetKlinesCall>();
    public ISet<GetExchangeInfoCall> GetExchangeInfoCalls { get; } = new HashSet<GetExchangeInfoCall>();

    public BinanceResponse<IList<Bar>> GetKlines(string symbol, DateTime startTime, DateTime endTime, Period period, int limit) {
        GetKlinesCalls.Add(new GetKlinesCall(symbol, startTime, endTime, period, limit));
        if (limit is < 0 or > 1000) {
            throw new InvalidUserArgumentException("Invalid limit");
        }
        if (startTime > endTime) {
            throw new InvalidUserArgumentException("Invalid date range");
        }
        
        CheckResetUsedWeight();
        AssertWeightLimitIsNotExceeded(1);

        LastRequestAt = Now;

        var bars = new List<Bar>(limit);
        var startMs = startTime.Truncate(period.Time).ToUnixTimeMs();
        var endMs = endTime.Truncate(period.Time).ToUnixTimeMs();
        while (startMs <= endMs && bars.Count < limit) {
            bars.Add(new Bar(symbol, startMs, period.UnixMs, 10.0m, 10.0m, 10.0m, 10.0m, 10.0m));
            startMs += period.UnixMs;
        }

        return new BinanceResponse<IList<Bar>>(HttpStatusCode.OK, bars, UsedWeight, null);
    }
    
    public BinanceResponse<ExchangeInfo> GetExchangeInfo() {
        GetExchangeInfoCalls.Add(new GetExchangeInfoCall());
        
        CheckResetUsedWeight();
        AssertWeightLimitIsNotExceeded(10);

        LastRequestAt = Now;
        var exchangeInfo = new ExchangeInfo {
            RateLimits = new[] {
                new RateLimit {
                    Interval = "MINUTE",
                    IntervalNum = 1,
                    Limit = 1000,
                    RateLimitType = "REQUEST_WEIGHT"
                }
            }
        };

        return new BinanceResponse<ExchangeInfo>(HttpStatusCode.OK, exchangeInfo, UsedWeight, null);
    }
    
    public void Wait(TimeSpan time) {
        Now += time;
        WaitCalls.Add(new WaitCall(time));
    }

    private void CheckResetUsedWeight() {
        if (LastRequestAt.Minute != Now.Minute || LastRequestAt + TimeSpan.FromMinutes(1) <= Now) {
            UsedWeight = 0;
        }
    }

    private void AssertWeightLimitIsNotExceeded(int weight) {
        UsedWeight += weight;
        if (UsedWeight > WeightLimit) {
            throw new InvalidStateException("Weight limit has been exceeded");
        }
    }
}

public class WaitCall {
    public TimeSpan Time { get; }
    public WaitCall(TimeSpan time) {
        Time = time;
    }

    protected bool Equals(WaitCall other) {
        return Time.Equals(other.Time);
    }

    public override bool Equals(object? obj) {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((WaitCall)obj);
    }

    public override int GetHashCode() {
        return Time.GetHashCode();
    }

    public override string ToString() {
        return $"{nameof(Time)}: {Time}";
    }
}

// string symbol, DateTime startTime, DateTime endTime, Period period, int limit
public class GetKlinesCall {
    public string Symbol { get; }
    public DateTime StartTime { get; }
    public DateTime EndTime { get; }
    public Period Period { get; }
    public int Limit { get; }
    public GetKlinesCall(string symbol, DateTime startTime, DateTime endTime, Period period, int limit) {
        Symbol = symbol;
        StartTime = startTime;
        EndTime = endTime;
        Period = period;
        Limit = limit;
    }

    protected bool Equals(GetKlinesCall other) {
        return Symbol == other.Symbol && StartTime.Equals(other.StartTime) && EndTime.Equals(other.EndTime) && Period == other.Period && Limit == other.Limit;
    }

    public override bool Equals(object? obj) {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((GetKlinesCall)obj);
    }

    public override int GetHashCode() {
        return HashCode.Combine(Symbol, StartTime, EndTime, Period, Limit);
    }

    public override string ToString() {
        return $"{nameof(Symbol)}: {Symbol}, {nameof(StartTime)}: {StartTime}, {nameof(EndTime)}: {EndTime}, {nameof(Period)}: {Period}, {nameof(Limit)}: {Limit}";
    }
}

public class GetExchangeInfoCall {
    
}