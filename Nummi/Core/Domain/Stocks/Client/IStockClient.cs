using Nummi.Core.Domain.Stocks.Data;
using Nummi.Core.Domain.Stocks.Ordering;

namespace Nummi.Core.Domain.Stocks.Client; 

public interface IStockClient {
    Task<Snapshot> GetSnapshotAsync(string symbol);
    Task<Snapshot> GetCryptoSnapshotAsync(string symbol);
    Task<Order> PlaceOrderAsync(PlaceOrderRq request);
}