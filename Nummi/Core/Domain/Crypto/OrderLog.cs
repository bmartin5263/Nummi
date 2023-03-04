using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto;

public readonly record struct OrderLogId(Guid Value) {
    public override string ToString() => Value.ToString("N");
    public static OrderLogId Generate() => new(Guid.NewGuid());
    public static OrderLogId FromGuid(Guid id) => new(id);
    public static OrderLogId FromString(string s) => new(Guid.ParseExact(s, "N"));
}

public class OrderLog {
    
    public OrderLogId Id { get; } = OrderLogId.Generate();
    
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