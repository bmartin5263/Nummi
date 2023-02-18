using System.ComponentModel.DataAnnotations;

namespace Nummi.Core.Domain.New; 

public class LimitOrderRq {
    
    [Required]
    public required string Symbol { get; init; }
    
    [Required]
    public required CryptoOrderQuantity Quantity { get; init; }
    
    [Required]
    public required decimal LimitPrice { get; init; }
    
    [Required]
    public required OrderSide Side { get; init; }
    
    [Required]
    public required TimeInForce Duration { get; init; }

    public OrderRequest ToOrderRequest() {
        return new OrderRequest {
            Symbol = Symbol, 
            Quantity = Quantity,
            Side = Side, 
            Type = OrderType.Limit, 
            Duration = Duration,
            LimitPrice = LimitPrice
        };
    }
}