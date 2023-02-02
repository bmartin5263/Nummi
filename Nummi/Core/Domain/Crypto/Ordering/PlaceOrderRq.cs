namespace Nummi.Core.Domain.Crypto.Ordering; 

public class PlaceOrderRq {
    public required string Symbol { get; init; } = default!;                 // AAPL
    public required CryptoOrderQuantity Quantity { get; init; }              // # of shares / $ amount
    public required OrderSide Side { get; init; }                            // Buy / Sell
    public required OrderType Type { get; init; }                            // Market/Stop/Limit/StopLimit/TrailingStop
    public required TimeInForce Duration { get; init; }                      // Day/GTC(Good til' Cancelled)/OPG(at open)/IOC/FOK/CLS
    public decimal? LimitPrice { get; init; }
    public decimal? StopPrice { get; init; }
    public string? ClientOrderId { get; init; }
    public bool? ExtendedHours { get; init; }
}