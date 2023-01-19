namespace Nummi.Api.Model; 

public class StrategyDto {
    public string? Id { get; set; }
    public TimeSpan? Frequency { get; set; }
    public bool? Initialized { get; set; }
    public decimal? Profit { get; set; }
    public uint TimesExecuted { get; set; }
    public DateTime? LastExecutedAt { get; set; }
    public uint? TimesFailed { get; set; }
    public object? Parameters { get; set; }
    public IList<StrategyLogDto> Logs { get; set; } = new List<StrategyLogDto>();
}