using Nummi.Core.App.Client;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.App;

public class TradingContext : ITradingContext {
    public IClock Clock { get; }
    public decimal RemainingFunds => FundSource.RemainingFunds;
    public TradingMode Mode { get; }
    private object Parameters { get; }
    private object State { get; }

    private IFundSource FundSource { get; }
    private ICryptoDataClient DataClient { get; }
    private ICryptoTradingClient TradingClient { get; }
    
    public TradingContext(
        TradingMode mode,
        IFundSource fundSource, 
        ICryptoDataClient dataClient, 
        ICryptoTradingClient tradingClient, 
        IClock clock
    ) {
        Mode = mode;
        DataClient = dataClient;
        TradingClient = tradingClient;
        Clock = clock;
        FundSource = fundSource;
        Parameters = null!;
        State = null!;
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

    public P GetParameters<P>() {
        return (P) Parameters;
    }

    public S GetState<S>() {
        return (S) State;
    }
}

public class TradingSession : ITradingSession {
    public IClock Clock { get; }
    public IFundSource FundSource { get; }
    public TradingMode Mode { get; }

    private ICryptoDataClient DataClient { get; }
    private ICryptoTradingClient TradingClient { get; }
    
    public TradingSession(
        IClock clock,
        TradingMode mode,
        IFundSource fundSource, 
        ICryptoDataClient dataClient, 
        ICryptoTradingClient tradingClient
    ) {
        Clock = clock;
        Mode = mode;
        FundSource = fundSource;
        DataClient = dataClient;
        TradingClient = tradingClient;
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