using Nummi.Core.Client;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.New.Data;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New;

public class TradingContext : ITradingContext {
    public IClock Clock { get; }
    public decimal RemainingFunds => FundSource.RemainingFunds;
    public TradingMode Mode { get; }
    public Ksuid? BotId { get; }

    private IFundSource FundSource { get; }
    private ICryptoDataClient DataClient { get; }
    private ICryptoTradingClient TradingClient { get; }
    
    public TradingContext(
        Ksuid? botId,
        TradingMode mode,
        IFundSource fundSource, 
        ICryptoDataClient dataClient, 
        ICryptoTradingClient tradingClient, 
        IClock clock
    ) {
        BotId = botId;
        Mode = mode;
        DataClient = dataClient;
        TradingClient = tradingClient;
        Clock = clock;
        FundSource = fundSource;
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