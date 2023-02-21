namespace Nummi.Api.Model; 

public class StrategyDto {
    public string? Id { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public StrategyTemplateVerionDto? ParentTemplate { get; set; }
    public string? ParametersJson { get; set; }
    public string? StateJson { get; set; }
    protected object? ParametersInstance { get; set; }
    public List<StrategyLogDto> Logs { get; set; } = new();
}