namespace Nummi.Core.Domain.Crypto.Bot.Execution; 

public class BotExecutionContext {
    public TradingBot Bot { get; }
    public IServiceScope Scope { get; }
    public CancellationToken CancellationToken { get; }

    public BotExecutionContext(TradingBot bot, IServiceScope scope, CancellationToken cancellationToken) {
        Bot = bot;
        CancellationToken = cancellationToken;
        Scope = scope;
    }

    public T? GetService<T>() {
        return Scope.ServiceProvider.GetService<T>();
    }
}