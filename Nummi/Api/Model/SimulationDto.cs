using Nummi.Core.Domain.New;

namespace Nummi.Api.Model; 

public class SimulationDto {
    public string? Id { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public string? StrategyId { get; set; }
    public StrategyDto? Strategy { get; set; }
    public SimulationState? State { get; set; }
    public DateTimeOffset? SimulationStartDate { get; set; }
    public DateTimeOffset? SimulationEndDate { get; set; }
    public DateTimeOffset? StartedAt { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }
    public TimeSpan? TotalExecutionTime { get; set; }
    public string? Error { get; set; }
}