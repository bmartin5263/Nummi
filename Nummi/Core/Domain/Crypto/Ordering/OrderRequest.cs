namespace Nummi.Core.Domain.Crypto.Ordering; 

public class OrderRequest {
    public required string Symbol { get; init; } = default!;                 // AAPL
    public required CryptoOrderQuantity Quantity { get; init; }              // # of shares / $ amount
    public required OrderSide Side { get; init; }                            // Buy / Sell
    public required OrderType Type { get; init; }                            // Market/Stop/Limit/StopLimit/TrailingStop
    public required TimeInForce Duration { get; init; }                      // Day/GTC(Good til' Cancelled)/OPG(at open)/IOC/FOK/CLS
    public decimal? LimitPrice { get; init; }
    public decimal? StopPrice { get; init; }

    public override string ToString() {
        return $"{nameof(Symbol)}: {Symbol}, {nameof(Quantity)}: {Quantity}, {nameof(Side)}: {Side}, {nameof(Type)}: {Type}, {nameof(Duration)}: {Duration}, {nameof(LimitPrice)}: {LimitPrice}, {nameof(StopPrice)}: {StopPrice}";
    }
}