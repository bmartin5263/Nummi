using System.ComponentModel.DataAnnotations;

namespace Nummi.Core.Domain.Crypto.Ordering; 

public class LimitOrderRq {
    
    [Required]
    public required string Symbol { get; init; }
    
    [Required]
    public required CryptoOrderQuantity Quantity { get; init; }
    
    [Required]
    public required decimal Price { get; init; }
    
    [Required]
    public required OrderSide Side { get; init; }
    
    [Required]
    public required TimeInForce Duration { get; init; }

    public PlaceOrderRq ToPlaceOrderRq() {
        return new PlaceOrderRq {
            Symbol = Symbol, 
            Quantity = Quantity,
            Side = Side, 
            Type = OrderType.Limit, 
            Duration = Duration,
            LimitPrice = Price
        };
    }
}