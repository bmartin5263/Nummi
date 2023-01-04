using System.Diagnostics.CodeAnalysis;
using Alpaca.Markets;

namespace TestWebApp.Api.Model; 

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")] // used by serializer
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class TradeDto {
    public string? Symbol { get; set; }
    public DateTime? TimestampUtc { get; set; }
    public decimal? Price { get; set; }
    public decimal? Size { get; set; }
    public ulong? TradeId { get; set; }
    public string? Exchange { get; set; }
    public string? Tape { get; set; }
    public string? Update { get; set; }
    public IReadOnlyList<string>? Conditions { get; set; }
    public TakerSide? TakerSide { get; set; }
}