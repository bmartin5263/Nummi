using Alpaca.Markets;
using Nummi.Core.Exceptions;

namespace Nummi.Core.External.Alpaca; 

public interface IAlpacaClient {
    
    protected IAlpacaDataClient DataClient { get; }
    protected IAlpacaCryptoDataClient CryptoDataClient { get; }
    protected IAlpacaTradingClient TradingClient { get; }

    public async Task<IAccount> GetTradingAccountAsync() {
        return await TradingClient.GetAccountAsync();
    }

    public async Task<IPage<IBar>> ListHistoricalBarsAsync(string symbol, DateTime? from, DateTime? into, BarTimeFrame timeFrame) {
        try {
            return await DataClient.ListHistoricalBarsAsync(new HistoricalBarsRequest(
                symbol, 
                from ?? DateTime.Now.Date.Subtract(TimeSpan.FromDays(1)), 
                into ?? DateTime.Now.Subtract(TimeSpan.FromMinutes(15)), 
                timeFrame
            ));
        }
        catch (Exception e) {
            throw new ExternalClientException("Failed to Get Historical Bars", e);
        }
    }

    public async Task<IMultiPage<IBar>> GetHistoricalBarsAsync(string symbol,BarTimeFrame timeFrame, DateTime? from = null, DateTime? into = null) {
        try {
            return await DataClient.GetHistoricalBarsAsync(new HistoricalBarsRequest(
                symbol, 
                from ?? DateTime.Now.Date.Subtract(TimeSpan.FromDays(1)), 
                into ?? DateTime.Now.Subtract(TimeSpan.FromMinutes(15)), 
                timeFrame
            ));
        }
        catch (Exception e) {
            throw new ExternalClientException("Failed to Get Historical Bars", e);
        }
    }

    public async Task<IBar> GetLatestBarAsync(string symbol) {
        try {
            return await DataClient.GetLatestBarAsync(new LatestMarketDataRequest(symbol));
        }
        catch (Exception e) {
            throw new ExternalClientException("Failed to Get Latest Bar", e);
        }
    }

    public async Task<ISnapshot> GetSnapshotAsync(string symbol) {
        try {
            return await DataClient.GetSnapshotAsync(new LatestMarketDataRequest(symbol));
        }
        catch (Exception e) {
            throw new ExternalClientException("Failed to Get Snapshot", e);
        }
    }

    public async Task<ISnapshot> GetCryptoSnapshotAsync(string symbol) {
        var list = new List<string>() { symbol };
        try {
            var result = await CryptoDataClient.ListSnapshotsAsync(new SnapshotDataListRequest(list));
            return result[symbol];
        }
        catch (Exception e) {
            throw new ExternalClientException("Failed to Get Snapshot", e);
        }
    }

    public async Task<IOrder> PostOrderAsync(NewOrderRequest request) {
        try {
            return await TradingClient.PostOrderAsync(request);
        }
        catch (Exception e) {
            throw new ExternalClientException("Failed to Post Order", e);
        }
        
    }
}