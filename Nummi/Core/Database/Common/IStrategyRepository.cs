using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Strategies;

namespace Nummi.Core.Database.Common; 

public interface IStrategyRepository : IGenericRepository<Ksuid, Strategy> {
    
}