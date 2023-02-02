using Nummi.Core.Domain.Crypto.Ordering;
using Nummi.Core.External.Alpaca;

namespace Nummi.Core.Domain.Crypto.Client; 

public class CryptoTradingClientRealtime : ICryptoTradingClient {

    private IAlpacaClient Client { get; }

    public CryptoTradingClientRealtime(IAlpacaClient client) {
        Client = client;
    }

    public async Task<Order> PlaceOrderAsync(PlaceOrderRq request) {
        var result = await Client.PostOrderAsync(request.ToAlpaca());
        return result.ToDomain();
    }
}