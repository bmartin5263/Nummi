using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Domain.Crypto.Ordering;

namespace Nummi.Core.Domain.Crypto.Client; 

public interface IStockClient {
    Task<Snapshot> GetSnapshotAsync(string symbol);
    Task<Snapshot> GetCryptoSnapshotAsync(string symbol);
    Task<Order> PlaceOrderAsync(PlaceOrderRq request);
}