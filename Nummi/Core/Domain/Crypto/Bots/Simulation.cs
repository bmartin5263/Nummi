using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using KSUID;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Domain.Crypto.Bots.Thread;
using Nummi.Core.Domain.Crypto.Log;
using Nummi.Core.Domain.Crypto.Strategies;
using Nummi.Core.Exceptions;

namespace Nummi.Core.Domain.Crypto.Bots; 

[Table("Simulation")]
[PrimaryKey(nameof(Id))]
public class Simulation {
    
    public string Id { get; }
    
    public Strategy Strategy { get; }
    
    public SimulationState State { get; private set; }
    
    public DateTime? StartTime { get; set; }
    
    public DateTime? EndTime { get; set; }
    
    public string? Error { get; private set; }

    [Column(nameof(TotalTime))]
    [SuppressMessage("ReSharper", "ValueParameterNotUsed", Justification = "Needed for Get-only properties")]
    public TimeSpan? TotalTime {
        get => EndTime - StartTime;
        private set { }
    }

    public List<StrategyLog> Logs { get; private set; } = new();

    private Simulation(): this(null!) { }

    public Simulation(Strategy strategy) {
        Id = Ksuid.Generate().ToString();
        State = SimulationState.Submitted;
        Strategy = strategy;
    }

    public void Start() {
        if (State != SimulationState.Submitted) {
            throw new InvalidStateException($"Can only start Simulations that are in {nameof(SimulationState.Submitted)} status");
        }

        State = SimulationState.Started;
        StartTime = DateTime.UtcNow;
    }

    public void Finish(Exception error) {
        if (State != SimulationState.Started) {
            throw new InvalidStateException($"Can only finish Simulations that are in {nameof(SimulationState.Started)} status");
        }
        
        EndTime = DateTime.UtcNow;
        State = SimulationState.Failed;
        Error = error.ToString();
    }

    public void Finish(IEnumerable<StrategyLog> logs) {
        if (State != SimulationState.Started) {
            throw new InvalidStateException($"Can only finish Simulations that are in {nameof(SimulationState.Started)} status");
        }

        Logs.AddRange(logs);
        EndTime = DateTime.UtcNow;
        State = SimulationState.Finished;
    }

    public void AddLogs(IEnumerable<StrategyLog> logs) {
        Logs.AddRange(logs);
    }
}