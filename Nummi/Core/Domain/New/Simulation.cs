using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using KSUID;
using Nummi.Core.Domain.Common;
using Nummi.Core.Exceptions;

namespace Nummi.Core.Domain.New;

public enum SimulationState {
    Created,
    Started,
    Finished
}

public class Simulation : Audited {

    public Ksuid Id { get; } = Ksuid.Generate();

    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }

    public Strategy Strategy { get; }

    public SimulationState State { get; private set; } = SimulationState.Created;
    
    public DateTimeOffset SimulationStartDate { get; set; }
    
    public DateTimeOffset SimulationEndDate { get; set; }
    
    public DateTimeOffset? StartedAt { get; set; }
    
    public DateTimeOffset? FinishedAt { get; set; }
    
    public string? Error { get; private set; }

    [Column(nameof(TotalExecutionTime))]
    [SuppressMessage("ReSharper", "ValueParameterNotUsed", Justification = "Needed for Get-only properties")]
    public TimeSpan? TotalExecutionTime {
        get => FinishedAt - StartedAt;
        private set { }
    }

    protected Simulation() {
        Strategy = null!;
    }
    
    public Simulation(Strategy strategy, DateTimeOffset startDate, DateTimeOffset endDate) {
        Strategy = strategy;
    }
    
    public void Start() {
        if (State != SimulationState.Created) {
            throw new InvalidStateException($"Can only start Simulations that are in {nameof(SimulationState.Created)} status");
        }

        State = SimulationState.Started;
        StartedAt = DateTimeOffset.UtcNow;
    }

    public void Finish(Exception error) {
        if (State != SimulationState.Started) {
            throw new InvalidStateException($"Can only finish Simulations that are in {nameof(SimulationState.Started)} status");
        }
        
        FinishedAt = DateTimeOffset.UtcNow;
        State = SimulationState.Finished;
        Error = error.ToString();
    }
}