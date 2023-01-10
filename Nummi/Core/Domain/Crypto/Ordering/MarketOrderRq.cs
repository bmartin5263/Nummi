using Alpaca.Markets;

namespace Nummi.Core.Domain.Crypto.Ordering; 

public class MarketOrderRq {
    public string Symbol { get; } = default!;                 // AAPL
    public decimal Shares { get; } = default!;        // # of shares / $ amount
    public OrderSide Side { get; } = default!;                // Buy / Sell
    public TimeInForce Duration { get; } = default!;          // Day/GTC(Good til' Cancelled)/OPG(at open)/IOC/FOK/CLS

    public MarketOrderRq() { }

    public MarketOrderRq(string symbol, decimal shares, OrderSide side, TimeInForce duration) {
        Symbol = symbol;
        Shares = shares;
        Side = side;
        Duration = duration;
    }

    public PlaceOrderRq ToPlaceOrderRq() {
        return new PlaceOrderRq(Symbol, OrderQuantity.Fractional(Shares), Side, OrderType.Market, Duration);
    }
}