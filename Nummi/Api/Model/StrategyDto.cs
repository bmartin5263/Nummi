namespace Nummi.Api.Model; 

public class StrategyDto {
    public string? Id { get; set; }
    public TimeSpan? Frequency { get; set; }
    public object? Parameters { get; set; }
}