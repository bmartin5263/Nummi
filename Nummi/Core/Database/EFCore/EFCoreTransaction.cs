using NLog;
using Nummi.Core.Database.Common;
using Nummi.Core.Util;

namespace Nummi.Core.Database.EFCore; 

public sealed class EFCoreTransaction : ITransaction {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public object DbContext { get; }
    public EFCoreContext EfCoreContext => (DbContext as EFCoreContext)!;
    
    private bool Disposed { get; set; }
    private DateTimeOffset StartTime { get; }

    public EFCoreTransaction(EFCoreContext appDb) {
        Log.Info($"-- Starting Transaction ({GetHashCode()}) --".Yellow());
        DbContext = appDb;
        StartTime = DateTimeOffset.Now;
    }
    
    public void Commit() {
        // Log.Info($"-- Commit --".Green());
        EfCoreContext.SaveChanges();
    }

    public void SaveAndDispose() {
        Commit();
        Dispose();
    }
    
    public void Dispose() {
        if (Disposed) {
            return;
        }
        Commit();
        Log.Info($"-- Disposing ({GetHashCode()}) ({DateTimeOffset.Now - StartTime}) --".Green());
        Disposed = true;
    }
}