using Nummi.Core.Domain.New;

namespace Nummi.Core.Client; 

public class CryptoTradingClientSimulated : ICryptoTradingClient {
    
    public CryptoTradingClientSimulated() {
    }

    public Task<Order> PlaceOrderAsync(OrderRequest request) {
        throw new NotImplementedException();
    }
}