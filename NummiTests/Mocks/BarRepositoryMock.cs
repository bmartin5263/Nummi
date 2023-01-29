using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Exceptions;

namespace NummiTests.Mocks; 

public class BarRepositoryMock : IBarRepository {

    public Dictionary<BarKey, Bar> Database { get; } = new();

    public Bar? FindById(string symbol, long openTimeUnixMs, long periodUnixMs) {
        var key = new BarKey(symbol, openTimeUnixMs, periodUnixMs);
        return Database.GetValueOrDefault(key);
    }

    public List<Bar> FindByIdRange(string symbol, long startOpenTimeUnixMs, long endOpenTimeUnixMs, long periodUnixMs) {
        return Database
            .Where(e =>
                e.Key.Symbol == symbol
                && e.Key.PeriodUnixMs == periodUnixMs
                && e.Key.OpenTimeUnixMs >= startOpenTimeUnixMs
                && e.Key.OpenTimeUnixMs <= endOpenTimeUnixMs
            )
            .Select(e => e.Value)
            .ToList();
    }

    public void Add(Bar bar) {
        var key = new BarKey(bar.Symbol, bar.OpenTimeUnixMs, bar.PeriodMs);
        if (Database.ContainsKey(key)) {
            throw new InvalidArgumentException($"Bar already exists {bar}");
        }

        Database[key] = bar;
    }

    public void Add(IDictionary<string, List<Bar>> barDict) {
        foreach (List<Bar> barList in barDict.Values) {
            foreach (Bar bar in barList) {
                var key = new BarKey(bar.Symbol, bar.OpenTimeUnixMs, bar.PeriodMs);
                if (Database.ContainsKey(key)) {
                    throw new InvalidArgumentException($"Bar already exists {bar}");
                }
                Database[key] = bar;
            }
        }
    }

    public void Save() {
        
    }
}

public class BarKey {
    public string Symbol { get; }
    public long OpenTimeUnixMs { get; }
    public long PeriodUnixMs { get; }
    public BarKey(string symbol, long openTimeUnixMs, long periodUnixMs) {
        Symbol = symbol;
        OpenTimeUnixMs = openTimeUnixMs;
        PeriodUnixMs = periodUnixMs;
    }

    protected bool Equals(BarKey other) {
        return Symbol == other.Symbol && OpenTimeUnixMs == other.OpenTimeUnixMs && PeriodUnixMs == other.PeriodUnixMs;
    }

    public override bool Equals(object? obj) {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BarKey)obj);
    }

    public override int GetHashCode() {
        return HashCode.Combine(Symbol, OpenTimeUnixMs, PeriodUnixMs);
    }
}