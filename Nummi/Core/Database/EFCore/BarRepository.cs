using Nummi.Core.Database.Common;
using Nummi.Core.Domain.New;

namespace Nummi.Core.Database.EFCore; 

public class BarRepository : IBarRepository {
    
    private EFCoreContext AppDb { get; }

    public BarRepository(EFCoreContext appDb) {
        AppDb = appDb;
    }

    public Bar? FindById(string symbol, DateTimeOffset openTime, TimeSpan period) {
        return AppDb.HistoricalBars.Find(symbol, openTime, period);
    }

    public List<Bar> FindByIdRange(string symbol, DateTimeOffset startUtc, DateTimeOffset endUtc, TimeSpan period) {
        return AppDb.HistoricalBars
            .Where(b => 
                b.Symbol == symbol
                && b.Period == period
                && b.OpenTime >= startUtc
                && b.OpenTime <= endUtc
            )
            .OrderBy(b => b.OpenTime)
            .ToList();
    }

    public void Add(Bar bar) {
        AppDb.HistoricalBars.Add(bar);
    }


    public int AddRange(IEnumerable<Bar> bars) {
        return AppDb.HistoricalBars.AddRangeIfNotExists(bars);
    }

    public void Save() {
        AppDb.SaveChanges();
    }
}