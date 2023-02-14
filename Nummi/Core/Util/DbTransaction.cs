using Microsoft.EntityFrameworkCore;
using NLog;
using Nummi.Core.Database.EFCore;

namespace Nummi.Core.Util; 

public class DbTransaction : IDisposable {
    
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly EFCoreContext appDb;
    
    public DbTransaction(EFCoreContext appDb) {
        this.appDb = appDb;
    }

    public void Dispose() {
        try {
            appDb.SaveChanges();
        }
        catch (DbUpdateException e) {
            Log.Info(e.Message);
        }
    }
}