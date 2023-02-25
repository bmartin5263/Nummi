using Nummi.Core.Domain.Crypto;

namespace Nummi.Core.App.Client; 

public class CryptoTradingClientSimulated : ICryptoTradingClient {
    
    public CryptoTradingClientSimulated() {
    }

    public Task<Order> PlaceOrderAsync(OrderRequest request) {
        throw new NotImplementedException();
    }
}