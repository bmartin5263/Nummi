using Nummi.Core.Domain.New;

namespace Nummi.Api.Model; 

public class StrategyLogDto {
    public string? Id { get; set; }
    public string? BotId { get; set; }
    public string? StrategyId { get; set; }
    public TradingMode? Environment { get; set; }
    public DateTimeOffset? StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public TimeSpan? TotalTime => EndTime - StartTime;
    public string? Error { get; set; }
}