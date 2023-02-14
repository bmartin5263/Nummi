using KSUID;
using Nummi.Core.Database.Common;
using Nummi.Core.Domain.New;

namespace Nummi.Core.Database.EFCore; 

public class StrategyTemplateRepository : GenericRepository<Ksuid, StrategyTemplate>, IStrategyTemplateRepository {
    public StrategyTemplateRepository(EFCoreContext context) : base(context) { }
}