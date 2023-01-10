using Alpaca.Markets;

namespace Nummi.Core.Domain.Crypto.Ordering; 

public class LimitOrderRq {
    public string Symbol { get; } = default!;
    public decimal Shares { get; } = default!;
    public decimal Price { get; } = default!;
    public OrderSide Side { get; } = default!;
    public TimeInForce Duration { get; } = default!;

    public LimitOrderRq() { }

    public LimitOrderRq(string symbol, decimal shares, decimal price, OrderSide side, TimeInForce duration) {
        Symbol = symbol;
        Shares = shares;
        Price = price;
        Side = side;
        Duration = duration;
    }

    public PlaceOrderRq ToPlaceOrderRq() {
        return new PlaceOrderRq(
            Symbol, 
            OrderQuantity.Fractional(Shares), 
            Side, 
            OrderType.Limit, 
            Duration,
            limitPrice:Price
        );
    }
}