using Nummi.Core.Domain.Crypto.Ordering;

namespace Nummi.Core.Domain.Crypto.Client; 

public interface ICryptoTradingClient {
    Task<Order> PlaceOrderAsync(OrderRequest request);
}