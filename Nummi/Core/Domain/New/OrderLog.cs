using KSUID;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New; 

public class OrderLog {
    
    public Ksuid Id { get; } = Ksuid.Generate();
    
    public required DateTime SubmittedAt { get; init; }

    public required string Symbol { get; init; }
    
    public required CryptoOrderQuantity Quantity { get; init; }
    
    public required OrderSide Side { get; init; }
    
    public required OrderType Type { get; init; }
    
    public required TimeInForce Duration { get; init; }
    
    public decimal? FundsBefore { get; init; }
    
    public decimal? FundsAfter { get; init; }
    
    public string? Error { get; init; }

    public override string ToString() {
        return this.ToFormattedString();
    }
}