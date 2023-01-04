using Nummi.Core.Domain.Stocks.Bot.Execution;

namespace Nummi.Core.Domain.Stocks.Bot.Strategy; 

public interface ITradingStrategy {

    public void Action(BotExecutionContext context);
    public Task Sleep(BotExecutionContext context);

}