namespace Nummi.Core.Bridge.DotNet; 

public class DotNetScope : INummiScope {
    private IServiceScope Scope { get; }
    private Dictionary<Type, object> ServiceCache { get; } = new();

    public DotNetScope(IServiceScope scope) {
        Scope = scope;
    }

    public void Dispose() {
        Scope.Dispose();
    }

    public T GetScoped<T>() where T : notnull {
        Type serviceType = typeof(T);
        if (ServiceCache.TryGetValue(serviceType, out var service)) {
            return (T) service;
        }
        service = Scope.ServiceProvider.GetRequiredService<T>();
        ServiceCache[serviceType] = service;
        return (T) service;
    }
}