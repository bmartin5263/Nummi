using Nummi.Core.App.Client;
using Nummi.Core.App.Trading;
using Nummi.Core.Domain.Bots;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.App;

public class TradingSession : ITradingSession {
    public IClock Clock { get; }
    public IFundSource FundSource { get; }
    public TradingMode Mode { get; }
    public Bot? Bot { get; }

    private ICryptoDataClient DataClient { get; }
    private ICryptoTradingClient TradingClient { get; }
    
    public TradingSession(
        IClock clock,
        TradingMode mode,
        IFundSource fundSource, 
        ICryptoDataClient dataClient, 
        ICryptoTradingClient tradingClient, 
        Bot? bot
    ) {
        Clock = clock;
        Mode = mode;
        FundSource = fundSource;
        DataClient = dataClient;
        TradingClient = tradingClient;
        Bot = bot;
    }

    public IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period) { 
        var result = DataClient.GetBars(symbols, dateRange, period);
        return result;
    }

    public Order PlaceOrder(OrderRequest request) {
        if (request.Quantity.Coins != null) {
            throw new InvalidOperationException("Making requests using Coin amount is not supported at this time");
        }
        if (request.Quantity.Dollars > FundSource.RemainingFunds) {
            throw new InvalidOperationException($"Insufficient Funds (${FundSource.RemainingFunds}) For Order {request}");
        }
        var result = TradingClient.PlaceOrderAsync(request).Result;
        FundSource.SubtractFunds(request.Quantity.Dollars!.Value);
        return result;
    }
}