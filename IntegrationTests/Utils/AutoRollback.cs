using Nummi.Core.Database.EFCore;

namespace IntegrationTests.Utils; 

public class AutoRollback : IDisposable {
    private EFCoreContext DbContext { get; }
    
    public AutoRollback(EFCoreContext dbContext) {
        DbContext = dbContext;
        DbContext.Database.BeginTransaction();
    }

    public void Dispose() {
        DbContext.ChangeTracker.Clear();
    }
    
}