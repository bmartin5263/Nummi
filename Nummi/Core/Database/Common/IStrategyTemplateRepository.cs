using Nummi.Core.Domain.Strategies;
using Nummi.Core.Domain.User;

namespace Nummi.Core.Database.Common; 

public interface IStrategyTemplateRepository : IGenericRepository<StrategyTemplateId, StrategyTemplate> {
    public void RemoveAllByUserId(IdentityId userId);
}