using Nummi.Core.Domain.Crypto;
using Nummi.Core.External.Alpaca;

namespace Nummi.Core.App.Client; 

public class CryptoTradingClientRealtime : ICryptoTradingClient {

    private IAlpacaClient Client { get; }

    public CryptoTradingClientRealtime(IAlpacaClient client) {
        Client = client;
    }

    public async Task<Order> PlaceOrderAsync(OrderRequest request) {
        var result = await Client.PostOrderAsync(request.ToAlpaca());
        return result.ToDomain();
    }
}