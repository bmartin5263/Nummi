using TestWebApp.Core.Domain.Stocks.Bot.Execution;

namespace TestWebApp.Core.Domain.Stocks.Bot.Strategy;

public class TradingStrategy : ITradingStrategy {
    
    public void Action(BotExecutionContext context) {
        Console.WriteLine("Beep");
    }

    public Task Sleep(BotExecutionContext context) {
        return Task.Delay(TimeSpan.FromSeconds(1), context.CancellationToken);
    }
}