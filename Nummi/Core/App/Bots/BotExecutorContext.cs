using Nummi.Core.Bridge;
using Nummi.Core.Database.Common;
using Nummi.Core.Database.EFCore;
using Nummi.Core.Domain.Bots;

namespace Nummi.Core.App.Bots; 

public class BotExecutorContext {
    public INummiServiceProvider ServiceProvider { get; }
    public DateTimeOffset Now { get; }
    
    private INummiScope Scope { get; }
    private Dictionary<BotId, Bot> BotCache { get; } = new();

    public BotExecutorContext(INummiServiceProvider serviceProvider, INummiScope scope) {
        ServiceProvider = serviceProvider;
        Scope = scope;
        Now = DateTimeOffset.UtcNow;
    }

    public Bot? FindBot(BotId id) {
        return BotCache.GetOrInsertNullable(
            id, 
            () => Scope.GetService<IBotRepository>().FindByIdWithStrategyAndActivation(id)
        );
    }

    public List<Bot> FindAllActiveBots() {
        var bots = Scope.GetService<IBotRepository>().FindActiveWithStrategyAndActivation();
        foreach (var bot in bots) {
            BotCache[bot.Id] = bot;
        }
        return bots;
    }
}