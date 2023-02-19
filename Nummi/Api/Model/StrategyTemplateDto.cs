namespace Nummi.Api.Model; 

public class StrategyTemplateDto {
    public string? Id { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public string? Name { get; set; }
    public TimeSpan? Frequency { get; set; }
}