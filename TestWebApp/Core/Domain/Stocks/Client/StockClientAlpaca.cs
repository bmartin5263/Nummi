using Alpaca.Markets;
using TestWebApp.Core.Domain.Stocks.Data;
using TestWebApp.Core.Domain.Stocks.Ordering;
using TestWebApp.Core.External.Alpaca;

namespace TestWebApp.Core.Domain.Stocks.Client; 

public class StockClientAlpaca : IStockClient {

    private readonly IAlpacaClient client;

    public StockClientAlpaca(IAlpacaClient client) {
        this.client = client;
    }

    public async Task<Snapshot> GetSnapshotAsync(string symbol) {
        var result = await client.GetSnapshotAsync(symbol);
        return result.ToDomain();
    }

    public async Task<Snapshot> GetCryptoSnapshotAsync(string symbol) {
        var result = await client.GetSnapshotAsync(symbol);
        return result.ToDomain();
    }

    public async Task<Order> PlaceOrderAsync(PlaceOrderRq request) {
        var result = await client.PostOrderAsync(request.ToAlpaca());
        return result.ToDomain();
    }
}