using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Bridge.DotNet; 

public class DotNetServiceProvider : INummiServiceProvider {
    private IServiceProvider ServiceProvider { get; }
    
    public DotNetServiceProvider(IServiceProvider serviceProvider) {
        ServiceProvider = serviceProvider;
    }
    
    public T GetSingleton<T>() {
        return ServiceProvider.GetService<T>()
            .OrElseThrow(() => new InvalidSystemStateException($"Missing required service of type {typeof(T).Name}"));
    }

    public INummiScope CreateScope() {
        var scope = ServiceProvider.CreateScope();
        return new DotNetScope(scope);
    }
}