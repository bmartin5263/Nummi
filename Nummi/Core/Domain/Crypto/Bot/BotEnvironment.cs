using Nummi.Core.Database;

namespace Nummi.Core.Domain.Crypto.Bot; 

public class BotEnvironment {
    
    public IServiceProvider ServiceProvider { get; }
    public IServiceScope Scope { get; }
    public AppDb AppDb { get; }
    
    public BotEnvironment(IServiceProvider serviceProvider, IServiceScope scope, AppDb appDb) {
        ServiceProvider = serviceProvider;
        Scope = scope;
        AppDb = appDb;
    }

    public T GetService<T>() {
        return ServiceProvider.GetService<T>()!;
    }
}