using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Crypto.Bots.Thread;

namespace Nummi.Core.Database.EFCore;

public class BotThreadRepository : GenericRepository<uint, BotThreadEntity>, IBotThreadRepository {
    public BotThreadRepository(EFCoreContext context) : base(context) { }
}