using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.New;

namespace Nummi.Core.Database.EFCore; 

public class StrategyRepository : GenericRepository<Ksuid, Strategy>, IStrategyRepository {
    public StrategyRepository(EFCoreContext context) : base(context) { }
}