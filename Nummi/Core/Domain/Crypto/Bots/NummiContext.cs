using Nummi.Core.Domain.Crypto.Strategies;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Bots;

public class NummiContext {
    
    private IServiceProvider ServiceProvider { get; }
    private IServiceScope Scope { get; }
    public TradingContextFactory TradingContextFactory => Scope.ServiceProvider.GetService<TradingContextFactory>()!;

    public NummiContext(IServiceProvider serviceProvider, IServiceScope scope) {
        ServiceProvider = serviceProvider;
        Scope = scope;
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