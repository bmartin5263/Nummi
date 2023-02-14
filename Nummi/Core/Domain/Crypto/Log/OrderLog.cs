using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using KSUID;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Domain.Crypto.Ordering;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Log; 

[PrimaryKey(nameof(Id))]
[Table("StrategyOrderLog")]
[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Persisted precomputed values")]
public class OrderLog {
    
    public string Id { get; } = Ksuid.Generate().ToString();
    
    public required DateTime SubmittedAt { get; init; }

    public required string Symbol { get; init; }
    
    public required CryptoOrderQuantity Quantity { get; init; }
    
    public required OrderSide Side { get; init; }
    
    public required OrderType Type { get; init; }
    
    public required TimeInForce Duration { get; init; }
    
    public required decimal FundsBefore { get; init; }
    
    public required decimal FundsAfter { get; init; }
    
    public string? Error { get; init; }

    public override string ToString() {
        return this.ToFormattedString();
    }
}