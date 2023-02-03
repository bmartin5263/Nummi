using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using KSUID;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Domain.Crypto.Ordering;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Strategies.Log; 

[PrimaryKey(nameof(Id))]
[Table("StrategyOrderLog")]
[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Persisted precomputed values")]
public class OrderLog {
    
    public string Id { get; } = Ksuid.Generate().ToString();
    
    public required DateTime SubmittedAt { get; init; }

    public required string Symbol { get; init; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required CryptoOrderQuantity Quantity { get; init; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required OrderSide Side { get; init; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required OrderType Type { get; init; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required TimeInForce Duration { get; init; }
    
    public required decimal FundsBefore { get; init; }
    
    public required decimal FundsAfter { get; init; }
    
    public string? Error { get; init; }

    public override string ToString() {
        return this.ToFormattedString();
    }
}