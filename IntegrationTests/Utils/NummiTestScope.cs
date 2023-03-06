using Nummi.Core.Bridge;
using Nummi.Core.Database.EFCore;

namespace IntegrationTests.Utils; 

public class NummiTestScope : INummiScope {
    private INummiScope Delegate { get; }

    public NummiTestScope(INummiScope @delegate) {
        Delegate = @delegate;
    }

    public void Dispose() {
        Delegate.Dispose();
    }

    public T GetScoped<T>() where T : notnull {
       return Delegate.GetScoped<T>();
    }
    
    public AutoRollback AutoRollback() {
        return new AutoRollback(Delegate.GetScoped<EFCoreContext>());
    }
}