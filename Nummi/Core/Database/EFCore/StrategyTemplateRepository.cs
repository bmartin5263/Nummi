using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.New;

namespace Nummi.Core.Database.EFCore; 

public class StrategyTemplateRepository : GenericRepository<Ksuid, StrategyTemplate>, IStrategyTemplateRepository {
    public StrategyTemplateRepository(ITransaction context) : base(context) { }
}