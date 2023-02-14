using Nummi.Core.Domain.New;

namespace Nummi.Api.Model; 

public class BotDto {
    public string? Id { get; set; }
    public string? Name { get; set; }
    public TradingMode Mode { get; set; }
    public bool? InErrorState { get; set; }
    public decimal? Funds { get; set; }
    public StrategyDto? Strategy { get; set; }
}