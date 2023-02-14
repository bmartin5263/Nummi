using Nummi.Core.Domain.New;

namespace Nummi.Core.Domain.Crypto.Client; 

public interface ICryptoTradingClient {
    Task<Order> PlaceOrderAsync(OrderRequest request);
}