using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Database.EFCore; 

public class BarRepository : GenericRepository<BarId, Bar>, IBarRepository {

    public BarRepository(ITransaction transaction) : base(transaction) {
        
    }

    public override Bar? FindNullableById(BarId id) {
        return Context.HistoricalBars.Find(id.Symbol, id.OpenTime, id.Period);
    }
    
    public override Bar FindById(BarId id) {
        return Context.HistoricalBars.Find(id.Symbol, id.OpenTime, id.Period)
            .OrElseThrow(() => EntityNotFoundException<Bar>.IdNotFound(id));
    }
    
    public override Bar RequireById(BarId id) {
        return Context.HistoricalBars.Find(id)
            .OrElseThrow(() => new EntityMissingException<Bar>(id));
    }

    public List<Bar> FindByIdRange(string symbol, DateTimeOffset startUtc, DateTimeOffset endUtc, TimeSpan period) {
        return Context.HistoricalBars
            .Where(b => 
                b.Symbol == symbol
                && b.Period == period
                && b.OpenTime >= startUtc
                && b.OpenTime <= endUtc
            )
            .OrderBy(b => b.OpenTime)
            .ToList();
    }

    public override long AddRangeIfNotExists(IEnumerable<Bar> entities) {
        var allEntities = entities.ToList();
        var symbols = allEntities.Select(e => e.Symbol);
        var openTimes = allEntities.Select(e => e.OpenTime);
        var periods = allEntities.Select(e => e.Period);
        var dbSet = Context.HistoricalBars;
        
        var rawSelection = from entity in dbSet
            where symbols.Contains(entity.Symbol) && openTimes.Contains(entity.OpenTime) && periods.Contains(entity.Period)
            select entity;

        var refined = from entity in rawSelection.AsEnumerable()
            join pair in allEntities on new { entity.Symbol, OpenTimeUtc = entity.OpenTime, entity.Period }
                equals new { pair.Symbol, OpenTimeUtc = pair.OpenTime, pair.Period}
            select entity;

        var entitiesToAdd = allEntities.Except(refined).ToList();
        dbSet.AddRange(entitiesToAdd);

        return entitiesToAdd.Count;
    }
}