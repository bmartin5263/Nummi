using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Bridge.DotNet; 

public class AspDotNetServiceProvider : INummiServiceProvider {
    private IServiceProvider ServiceProvider { get; }
    
    public AspDotNetServiceProvider(IServiceProvider serviceProvider) {
        ServiceProvider = serviceProvider;
    }
    
    public T GetService<T>() {
        return ServiceProvider.GetService<T>()
            .OrElseThrow(() => new InvalidSystemStateException($"Missing required service of type {typeof(T).Name}"));
    }

    public INummiScope CreateScope() {
        var scope = ServiceProvider.CreateScope();
        return new AspDotNetScope(new AspDotNetServiceProvider(ServiceProvider.CreateScope().ServiceProvider), scope);
    }
}