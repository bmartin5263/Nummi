using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Strategies;

namespace Nummi.Core.Database.EFCore; 

public class StrategyRepository : GenericRepository<StrategyId, Strategy>, IStrategyRepository {
    public StrategyRepository(ITransaction context) : base(context) { }
}