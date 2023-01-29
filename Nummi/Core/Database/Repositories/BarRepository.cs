using Nummi.Core.Domain.Crypto.Data;

namespace Nummi.Core.Database.Repositories; 

public class BarRepository : IBarRepository {
    
    private AppDb AppDb { get; }

    public BarRepository(AppDb appDb) {
        AppDb = appDb;
    }

    public Bar? FindById(string symbol, long openTimeUnixMs, long periodUnixMs) {
        return AppDb.HistoricalBars.Find(symbol, openTimeUnixMs, periodUnixMs);
    }

    public List<Bar> FindByIdRange(string symbol, long startUnixMs, long endUnixMs, long periodUnixMs) {
        return AppDb.HistoricalBars
            .Where(b => 
                b.Symbol == symbol
                && b.PeriodMs == periodUnixMs
                && b.OpenTimeUnixMs >= startUnixMs
                && b.OpenTimeUnixMs <= endUnixMs
            )
            .OrderBy(b => b.OpenTimeUnixMs)
            .ToList();
    }

    public void Add(Bar bar) {
        AppDb.HistoricalBars.Add(bar);
    }

    public void Save() {
        AppDb.SaveChanges();
    }
}