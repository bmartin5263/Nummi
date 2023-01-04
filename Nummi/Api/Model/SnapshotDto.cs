using System.Diagnostics.CodeAnalysis;


namespace Nummi.Api.Model; 

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")] // used by serializer
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class SnapshotDto {
    public string? Symbol { get; set; }
    public QuoteDto? Quote { get; set; }
    public TradeDto? Trade { get; set; }
    public BarDto? MinuteBar { get; set; }
    public BarDto? CurrentDailyBar { get; set; }
    public BarDto? PreviousDailyBar { get; set; }
}