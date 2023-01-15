namespace Nummi.Api.Model; 

public class StockBotDto {
    public string? Id { get; set; }
    public string? Name { get; set; }
    public StrategyDto? Strategy { get; set; }
    public decimal? Funds { get; set; }
}