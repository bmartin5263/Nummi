using System.Diagnostics.CodeAnalysis;

namespace Nummi.Api.Model; 

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")] // used by serializer
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class BarDto {
    public string? Symbol { get; set; }
    public DateTimeOffset? OpenTimeUtc { get; set; }
    public DateTimeOffset? CloseTimeUtc { get; set; }
    public decimal? Open { get; set; }
    public decimal? High { get; set; }
    public decimal? Low { get; set; }
    public decimal? Close { get; set; }
    public decimal? Volume { get; set; }
}