namespace Nummi.Api.Model; 

public class StrategyDto {
    public string? Id { get; set; }
    public TimeSpan? Frequency { get; set; }
    public int TimesExecuted { get; set; }
    public DateTime? LastExecutedAt { get; set; }
    public int? TimesFailed { get; set; }
    public object? Parameters { get; set; }
    public IList<StrategyLogDto> Logs { get; set; } = new List<StrategyLogDto>();
}