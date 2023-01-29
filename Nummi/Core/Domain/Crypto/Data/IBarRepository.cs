namespace Nummi.Core.Domain.Crypto.Data; 

public interface IBarRepository {
    Bar? FindById(string symbol, long openTimeUnixMs, long periodUnixMs);
    List<Bar> FindByIdRange(string symbol, long startOpenTimeUnixMs, long endOpenTimeUnixMs, long periodUnixMs);
    void Add(Bar bar);
    int AddRange(IEnumerable<Bar> bars);
    void Save();
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