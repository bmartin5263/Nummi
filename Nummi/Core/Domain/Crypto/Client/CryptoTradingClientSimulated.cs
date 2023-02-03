using Nummi.Core.Domain.Crypto.Ordering;

namespace Nummi.Core.Domain.Crypto.Client; 

public class CryptoTradingClientSimulated : ICryptoTradingClient {
    
    public CryptoTradingClientSimulated() {
    }

    public Task<Order> PlaceOrderAsync(OrderRequest request) {
        throw new NotImplementedException();
    }
}