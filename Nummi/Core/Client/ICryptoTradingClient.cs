using Nummi.Core.Domain.New;

namespace Nummi.Core.Client; 

public interface ICryptoTradingClient {
    Task<Order> PlaceOrderAsync(OrderRequest request);
}