using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.New;
using Nummi.Core.Domain.Strategies;

namespace Nummi.Core.Database.EFCore; 

public class StrategyRepository : GenericRepository<Ksuid, Strategy>, IStrategyRepository {
    public StrategyRepository(ITransaction context) : base(context) { }
}