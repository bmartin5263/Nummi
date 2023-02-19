using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.New;

namespace Nummi.Core.Database.EFCore; 

public class BotRepository : GenericRepository<Ksuid, Bot>, IBotRepository {
    public BotRepository(ITransaction context) : base(context) { }
}