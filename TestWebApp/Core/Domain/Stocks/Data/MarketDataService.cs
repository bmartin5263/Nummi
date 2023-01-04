using TestWebApp.Core.Domain.Stocks.Client;

namespace TestWebApp.Core.Domain.Stocks.Data; 

public class MarketDataService {

    private readonly IStockClient stockClient;

    public MarketDataService(IStockClient stockClient) {
        this.stockClient = stockClient;
    }

    public Task<Snapshot> GetSnapshot(string symbol) {
        return stockClient.GetSnapshotAsync(symbol);
    }

    public Task<Snapshot> GetCryptoSnapshot(string symbol) {
        return stockClient.GetCryptoSnapshotAsync(symbol);
    }

}