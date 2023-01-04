using TestWebApp.Core.Domain.Stocks.Bot.Execution;

namespace TestWebApp.Core.Domain.Stocks.Bot.Strategy; 

public interface ITradingStrategy {

    public void Action(BotExecutionContext context);
    public Task Sleep(BotExecutionContext context);

}