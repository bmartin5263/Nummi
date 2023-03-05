using NLog;
using Nummi.Core.Database.Common;
using Nummi.Core.Util;

namespace Nummi.Core.Database.EFCore; 

public class EFCoreTransaction : ITransaction {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private bool Disposed { get; set; }
    public object DbContext { get; }

    public EFCoreTransaction(EFCoreContext appDb) {
        Log.Info($"-- Starting Transaction --".Yellow());
        DbContext = appDb;
    }
    
    public void Commit() {
        Log.Info($"-- Commit --".Green());
        (DbContext as EFCoreContext)!.SaveChanges();
    }

    public void SaveAndDispose() {
        Commit();
        Dispose();
    }
    
    public void Dispose() {
        Log.Info($"-- Disposing --".Red());
        if (Disposed) {
            return;
        }
        Commit();
        Disposed = true;
    }
}