using Nummi.Core.Domain.Stocks.Bot.Execution;

namespace Nummi.Core.Domain.Stocks.Bot.Strategy; 

public interface ITradingStrategy {
    public void Execute(BotExecutionContext context);
}