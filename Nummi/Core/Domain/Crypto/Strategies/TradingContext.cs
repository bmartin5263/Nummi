using Nummi.Core.Database;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto.Client;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Domain.Crypto.Ordering;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Strategies;

public class TradingContext : ITradingContext {
    public TradingMode Mode { get; private set; }
    public decimal Funds { get; private set; }
    public IClock Clock { get; private set; }
    
    protected ICryptoDataClient DataClient { get; private set; }
    protected ICryptoTradingClient TradingClient { get; private set; }
    protected AppDb AppDb { get; private set; }

    public TradingContext(TradingMode mode, ICryptoDataClient dataClient, ICryptoTradingClient tradingClient, decimal funds, AppDb appDb, IClock clock) {
        Mode = mode;
        DataClient = dataClient;
        TradingClient = tradingClient;
        Funds = funds;
        AppDb = appDb;
        Clock = clock;
    }

    public IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period) {
        return DataClient.GetBars(symbols, dateRange, period);
    }

    public Order PlaceOrder(OrderRequest request) {
        if (request.Quantity.Coins != null) {
            throw new InvalidOperationException("Making requests using Coin amount is not supported at this time");
        }
        if (request.Quantity.Dollars > Funds) {
            throw new InvalidOperationException($"Insufficient Funds (${Funds}) For Order {request}");
        }
        
        var result = TradingClient.PlaceOrderAsync(request).Result;
        Funds -= request.Quantity.Dollars!.Value;
        return result;
    }
}