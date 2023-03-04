using Nummi.Core.Domain.Crypto;
using Nummi.Core.Domain.Strategies;

namespace Nummi.Api.Model; 

public class StrategyLogDto {
    public string? Id { get; set; }
    public string? BotLogId { get; set; }
    public TradingMode? Mode { get; set; }
    public StrategyAction? Action { get; set; }
    public DateTimeOffset? StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public TimeSpan? TotalTime { get; set; }
    public int? ApiCalls { get; set; }
    public TimeSpan? TotalApiCallTime { get; set; }
    public string? Error { get; set; }
    public IList<OrderLogDto> Orders { get; set; } = new List<OrderLogDto>();
}