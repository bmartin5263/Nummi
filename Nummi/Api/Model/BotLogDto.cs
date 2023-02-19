namespace Nummi.Api.Model; 

public class BotLogDto {
    public string? Id { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public TimeSpan? TotalTime { get; set; }
    public string? Error { get; set; }
}