using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Nummi.Core.Domain.Crypto.Ordering; 

public class MarketOrderRq {
    
    [Required]
    [UsedImplicitly]
    public required string Symbol { get; init; }                 // AAPL
    
    [UsedImplicitly]
    public required CryptoOrderQuantity Quantity { get; init; }       // # of shares / $ amount
    
    [Required]
    [UsedImplicitly]
    public required OrderSide Side { get; init; }                // Buy / Sell
    
    [Required]
    [UsedImplicitly]
    public required TimeInForce Duration { get; init; }          // Day/GTC(Good til' Cancelled)/OPG(at open)/IOC/FOK/CLS

    public PlaceOrderRq ToPlaceOrderRq() {
        return new PlaceOrderRq {
            Symbol = Symbol, 
            Quantity = Quantity,
            Side = Side, 
            Type = OrderType.Market, 
            Duration = Duration
        };
    }
}