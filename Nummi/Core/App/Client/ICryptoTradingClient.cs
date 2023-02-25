using Nummi.Core.Domain.Crypto;

namespace Nummi.Core.App.Client; 

public interface ICryptoTradingClient {
    Task<Order> PlaceOrderAsync(OrderRequest request);
}