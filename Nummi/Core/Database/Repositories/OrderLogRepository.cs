using Nummi.Core.Domain.Crypto.Strategies.Log;

namespace Nummi.Core.Database.Repositories; 

public class OrderLogRepository : IOrderLogRepository {
    
    private AppDb AppDb { get; }

    public OrderLogRepository(AppDb appDb) {
        AppDb = appDb;
    }
    
    public void Add(OrderLog log) {
        AppDb.OrderLogs.Add(log);
    }

}