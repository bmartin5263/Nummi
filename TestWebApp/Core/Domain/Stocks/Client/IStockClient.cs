using TestWebApp.Core.Domain.Stocks.Data;
using TestWebApp.Core.Domain.Stocks.Ordering;

namespace TestWebApp.Core.Domain.Stocks.Client; 

public interface IStockClient {
    Task<Snapshot> GetSnapshotAsync(string symbol);
    Task<Snapshot> GetCryptoSnapshotAsync(string symbol);
    Task<Order> PlaceOrderAsync(PlaceOrderRq request);
}