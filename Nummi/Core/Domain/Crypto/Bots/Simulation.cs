using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using KSUID;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Domain.Crypto.Bots.Thread;
using Nummi.Core.Domain.Crypto.Strategies;
using Nummi.Core.Exceptions;

namespace Nummi.Core.Domain.Crypto.Bots; 

[Table("Simulation")]
[PrimaryKey(nameof(Id))]
public class Simulation {
    
    public string Id { get; }
    
    public SimulationStatus Status { get; private set; }
    
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
    
    public Simulation() {
        Id = Ksuid.Generate().ToString();
        Status = SimulationStatus.Waiting;
    }

    public void Start() {
        if (Status != SimulationStatus.Waiting) {
            throw new InvalidStateException($"Can only start Simulations that are in {nameof(SimulationStatus.Waiting)} status");
        }

        Status = SimulationStatus.Started;
        StartTime = DateTime.UtcNow;
    }

    public void Finish(Exception error) {
        if (Status != SimulationStatus.Started) {
            throw new InvalidStateException($"Can only finish Simulations that are in {nameof(SimulationStatus.Started)} status");
        }
        
        EndTime = DateTime.UtcNow;
        Status = SimulationStatus.Failed;
        Error = error.ToString();
    }

    public void Finish(IEnumerable<StrategyLog> logs) {
        if (Status != SimulationStatus.Started) {
            throw new InvalidStateException($"Can only finish Simulations that are in {nameof(SimulationStatus.Started)} status");
        }

        Logs.AddRange(logs);
        EndTime = DateTime.UtcNow;
        Status = SimulationStatus.Finished;
    }

    public void AddLogs(IEnumerable<StrategyLog> logs) {
        Logs.AddRange(logs);
    }
}