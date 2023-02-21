namespace Nummi.Api.Model; 

public class StrategyTemplateVerionDto {
    public uint? VersionNumber { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public string? Name { get; set; }
    public TimeSpan? Frequency { get; set; }
}