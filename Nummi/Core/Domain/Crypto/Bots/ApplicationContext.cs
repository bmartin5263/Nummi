using Nummi.Core.Database;

namespace Nummi.Core.Domain.Crypto.Bots; 

public class ApplicationContext {
    
    public IServiceProvider ServiceProvider { get; }
    public IServiceScope Scope { get; }
    public AppDb AppDb { get; }
    
    public ApplicationContext(IServiceProvider serviceProvider, IServiceScope scope, AppDb appDb) {
        ServiceProvider = serviceProvider;
        Scope = scope;
        AppDb = appDb;
    }

    public T GetSingleton<T>() {
        return ServiceProvider.GetService<T>()!;
    }

    public T GetScoped<T>() {
        return Scope.ServiceProvider.GetService<T>()!;
    }
}