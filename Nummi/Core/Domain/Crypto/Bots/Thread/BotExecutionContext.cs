namespace Nummi.Core.Domain.Crypto.Bots.Thread; 

public class BotExecutionContext {
    public Bot Bot { get; }
    public IServiceScope Scope { get; }
    public CancellationToken CancellationToken { get; }

    public BotExecutionContext(Bot bot, IServiceScope scope, CancellationToken cancellationToken) {
        Bot = bot;
        CancellationToken = cancellationToken;
        Scope = scope;
    }

    public T? GetService<T>() {
        return Scope.ServiceProvider.GetService<T>();
    }
}