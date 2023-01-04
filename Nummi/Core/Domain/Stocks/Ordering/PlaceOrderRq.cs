using Alpaca.Markets;

namespace Nummi.Core.Domain.Stocks.Ordering; 

public class PlaceOrderRq {
    public string Symbol { get; } = default!;                 // AAPL
    public OrderQuantity Quantity { get; } = default!;        // # of shares / $ amount
    public OrderSide Side { get; } = default!;                // Buy / Sell
    public OrderType Type { get; } = default!;                // Market/Stop/Limit/StopLimit/TrailingStop
    public TimeInForce Duration { get; } = default!;          // Day/GTC(Good til' Cancelled)/OPG(at open)/IOC/FOK/CLS
    public decimal? LimitPrice { get; }
    public decimal? StopPrice { get; }
    public decimal? TrailOffsetInDollars { get; }
    public decimal? TrailOffsetInPercent { get; }
    public string? ClientOrderId { get; }
    public bool? ExtendedHours { get; }
    public OrderClass? OrderClass { get; }
    public decimal? TakeProfitLimitPrice { get; }
    public decimal? StopLossStopPrice { get; }
    public decimal? StopLossLimitPrice { get; }

    public PlaceOrderRq() { }

    public PlaceOrderRq(
        string symbol, OrderQuantity quantity, OrderSide side, OrderType type, TimeInForce duration, 
        decimal? limitPrice = null, decimal? stopPrice = null, decimal? trailOffsetInDollars = null, 
        decimal? trailOffsetInPercent = null, string? clientOrderId = null, bool? extendedHours = null, 
        OrderClass? orderClass = null, decimal? takeProfitLimitPrice = null, decimal? stopLossStopPrice = null, 
        decimal? stopLossLimitPrice = null
    ) {
        Symbol = symbol;
        Quantity = quantity;
        Side = side;
        Type = type;
        Duration = duration;
        LimitPrice = limitPrice;
        StopPrice = stopPrice;
        TrailOffsetInDollars = trailOffsetInDollars;
        TrailOffsetInPercent = trailOffsetInPercent;
        ClientOrderId = clientOrderId;
        ExtendedHours = extendedHours;
        OrderClass = orderClass;
        TakeProfitLimitPrice = takeProfitLimitPrice;
        StopLossStopPrice = stopLossStopPrice;
        StopLossLimitPrice = stopLossLimitPrice;
    }
}