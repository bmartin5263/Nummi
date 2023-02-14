using Nummi.Core.Domain.New;
using Nummi.Core.External.Alpaca;

namespace Nummi.Core.Domain.Crypto.Client; 

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