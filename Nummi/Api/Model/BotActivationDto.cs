using Nummi.Core.Domain.Crypto;

namespace Nummi.Api.Model; 

public class BotActivationDto {
    public string? Id { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public StrategyDto? Strategy { get; set; }
    public TradingMode? Mode { get; set; }
    public List<BotLogDto> Logs { get; set; } = new();
}