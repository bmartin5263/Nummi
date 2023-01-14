using Nummi.Core.Database;

namespace Nummi.Core.Domain.Crypto.Bot; 

public class BotEnvironment {
    
    public IServiceProvider ServiceProvider { get; }
    public IServiceScope Scope { get; }
    public AppDb AppDb { get; }
    public ILogger Logger { get; }
    
    public BotEnvironment(IServiceProvider serviceProvider, IServiceScope scope, AppDb appDb, ILogger logger) {
        ServiceProvider = serviceProvider;
        Scope = scope;
        AppDb = appDb;
        Logger = logger;
    }

    public T GetService<T>() {
        return ServiceProvider.GetService<T>()!;
    }
}