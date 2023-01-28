using Nummi.Core.Database;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Bots;

public class BotContext {
    
    private IServiceProvider ServiceProvider { get; }
    private IServiceScope Scope { get; }
    public AppDb AppDb { get; }
    
    public BotContext(IServiceProvider serviceProvider, IServiceScope scope, AppDb appDb) {
        ServiceProvider = serviceProvider;
        Scope = scope;
        AppDb = appDb;
    }

    public T GetSingleton<T>() {
        return ServiceProvider.GetService<T>()
            .ThrowIfNull(() => new InvalidStateException($"Missing Singleton {typeof(T).FullName}"));
    }

    public T GetScoped<T>() {
        return Scope.ServiceProvider.GetService<T>()
            .ThrowIfNull(() => new InvalidStateException($"Missing Scoped {typeof(T).FullName}"));
    }
}