namespace Nummi.Core.Bridge.DotNet; 

public class AspDotNetScope : INummiScope {
    private INummiServiceProvider ServiceProvider { get; }
    private IServiceScope Scope { get; }

    public AspDotNetScope(INummiServiceProvider serviceProvider, IServiceScope scope) {
        ServiceProvider = serviceProvider;
        Scope = scope;
    }

    public void Dispose() {
        Scope.Dispose();
    }

    public T GetService<T>() {
        return ServiceProvider.GetService<T>();
    }
}