using Nummi.Core.Database;

namespace Nummi.Core.Util; 

public class DbTransaction : IDisposable {
    
    private readonly AppDb appDb;
    
    public DbTransaction(AppDb appDb) {
        this.appDb = appDb;
    }

    public void Dispose() {
        appDb.SaveChanges();
    }
}