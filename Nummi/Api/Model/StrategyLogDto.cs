using Nummi.Core.Domain.Crypto.Strategies;

namespace Nummi.Api.Model; 

public class StrategyLogDto {
    public string? Id { get; set; }
    public string? StrategyId { get; set; }
    public TradingEnvironment? Environment { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public TimeSpan? TotalTime => EndTime - StartTime;
    public string? Error { get; set; }
}