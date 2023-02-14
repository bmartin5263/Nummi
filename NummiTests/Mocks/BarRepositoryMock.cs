using Nummi.Core.Database.Common;
using Nummi.Core.Domain.New;
using Nummi.Core.Exceptions;

namespace NummiTests.Mocks; 

public class BarRepositoryMock : IBarRepository {

    public Dictionary<BarKey, Bar> Database { get; } = new();

    public Bar? FindById(string symbol, DateTimeOffset openTime, TimeSpan period) {
        var key = new BarKey(symbol, openTime, period);
        return Database.GetValueOrDefault(key);
    }

    public List<Bar> FindByIdRange(string symbol, DateTimeOffset startOpenTimeUtc, DateTimeOffset endOpenTimeUtc, TimeSpan period) {
        return Database
            .Where(e =>
                e.Key.Symbol == symbol
                && e.Key.Period == period
                && e.Key.OpenTimeUtc >= startOpenTimeUtc
                && e.Key.OpenTimeUtc <= endOpenTimeUtc
            )
            .Select(e => e.Value)
            .ToList();
    }

    public void Add(Bar bar) {
        var key = new BarKey(bar.Symbol, bar.OpenTime, bar.Period);
        if (Database.ContainsKey(key)) {
            throw new InvalidUserArgumentException($"Bar already exists {bar}");
        }

        Database[key] = bar;
    }

    public int AddRange(IEnumerable<Bar> bars) {
        int added = 0;
        foreach (var bar in bars) {
            try {
                Add(bar);
                ++added;
            }
            catch (InvalidUserArgumentException) {
            }
        }

        return added;
    }

    public void Add(IDictionary<string, List<Bar>> barDict) {
        foreach (List<Bar> barList in barDict.Values) {
            foreach (Bar bar in barList) {
                var key = new BarKey(bar.Symbol, bar.OpenTime, bar.Period);
                if (Database.ContainsKey(key)) {
                    throw new InvalidUserArgumentException($"Bar already exists {bar}");
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
    public DateTimeOffset OpenTimeUtc { get; }
    public TimeSpan Period { get; }
    public BarKey(string symbol, DateTimeOffset openTime, TimeSpan period) {
        Symbol = symbol;
        OpenTimeUtc = openTime;
        Period = period;
    }

    protected bool Equals(BarKey other) {
        return Symbol == other.Symbol && OpenTimeUtc == other.OpenTimeUtc && Period == other.Period;
    }

    public override bool Equals(object? obj) {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BarKey)obj);
    }

    public override int GetHashCode() {
        return HashCode.Combine(Symbol, OpenTimeUtc, Period);
    }
}