using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Strategies;

namespace Nummi.Core.Database.Common; 

public interface IStrategyTemplateRepository : IGenericRepository<Ksuid, StrategyTemplate> {
    public void RemoveAllByUserId(Ksuid userId);
}