namespace Nummi.Api.Model; 

public class StockBotDto {
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Strategy { get; set; }
    public decimal? AvailableCash { get; set; }
    public decimal? Profit { get; set; }
}