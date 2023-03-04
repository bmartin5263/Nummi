using Nummi.Core.Domain.Bots;

namespace Nummi.Core.Database.Common; 

public interface IBotRepository : IGenericRepository<BotId, Bot> {
    Bot? FindByIdWithStrategyAndActivation(BotId botId);
    List<Bot> FindActiveWithStrategyAndActivation();
    Bot? FindByIdForExecution(BotId botId);
}