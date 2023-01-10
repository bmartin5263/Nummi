using Nummi.Core.Domain.Crypto.Bot.Execution;

namespace Nummi.Core.Domain.Crypto.Bot.Strategy; 

public interface ITradingStrategy {
    public void Execute(BotExecutionContext context);
}