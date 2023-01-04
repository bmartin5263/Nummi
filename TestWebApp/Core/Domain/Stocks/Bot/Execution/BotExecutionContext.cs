namespace TestWebApp.Core.Domain.Stocks.Bot.Execution; 

public class BotExecutionContext {
    public CancellationToken CancellationToken { get; }

    public BotExecutionContext(CancellationToken cancellationToken) {
        CancellationToken = cancellationToken;
    }
}