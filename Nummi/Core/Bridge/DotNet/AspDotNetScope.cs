namespace Nummi.Core.Bridge.DotNet; 

public class AspDotNetScope : INummiScope {
    private INummiServiceProvider ServiceProvider { get; }
    private IServiceScope Scope { get; }
    private Dictionary<Type, object> ServiceCache { get; } = new();

    public AspDotNetScope(INummiServiceProvider serviceProvider, IServiceScope scope) {
        ServiceProvider = serviceProvider;
        Scope = scope;
    }

    public void Dispose() {
        Scope.Dispose();
    }

    public T GetService<T>() {
        Type serviceType = typeof(T);
        if (ServiceCache.TryGetValue(serviceType, out var service)) {
            return (T) service;
        }
        service = ServiceProvider.GetService<T>()!;
        ServiceCache[serviceType] = service;
        return (T) service;
    }
}