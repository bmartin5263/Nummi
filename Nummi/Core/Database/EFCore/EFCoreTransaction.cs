using Nummi.Core.Database.Common;

namespace Nummi.Core.Database.EFCore; 

public class EFCoreTransaction : ITransaction {
    private bool Disposed { get; set; }
    public object DbContext { get; }

    public EFCoreTransaction(EFCoreContext appDb) {
        DbContext = appDb;
    }
    
    public void Commit() {
        (DbContext as EFCoreContext)!.SaveChanges();
    }

    public void SaveAndDispose() {
        Commit();
        Dispose();
    }
    
    public void Dispose() {
        if (Disposed) {
            return;
        }
        GC.SuppressFinalize(this);
        Commit();
        Disposed = true;
    }

}