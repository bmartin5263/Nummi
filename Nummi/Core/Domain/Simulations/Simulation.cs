using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using NLog;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.Domain.Strategies;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Simulations;

public enum SimulationState {
    Created,
    Started,
    Finished
}

public class Simulation : Audited {
    
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public Ksuid Id { get; } = Ksuid.Generate();

    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }

    public Strategy Strategy { get; }

    public SimulationState State { get; private set; } = SimulationState.Created;
    
    public DateTimeOffset SimulationStartDate { get; private set; }
    
    public DateTimeOffset SimulationEndDate { get; private set; }
    
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
        SimulationStartDate = startDate.Truncate(TimeSpan.FromSeconds(1));
        SimulationEndDate = endDate.Truncate(TimeSpan.FromSeconds(1));
    }

    public void Start(ITradingSession session) {
        if (State != SimulationState.Created) {
            throw new InvalidSystemStateException($"Can only start Simulations that are in {nameof(SimulationState.Created)} status");
        }

        State = SimulationState.Started;
        StartedAt = DateTimeOffset.UtcNow;

        DateTimeOffset now = SimulationStartDate;
        ClockMock clock = (ClockMock) session.Clock;
        clock.NowUtc = now;

        Log.Info($"Running Simulation from {SimulationStartDate} to {SimulationEndDate}");
        try {
            Strategy.Initialize(session);
            while (clock.NowUtc < SimulationEndDate) {
                Strategy.CheckForTrades(session);
                clock.NowUtc += Strategy.Frequency;
            }
        }
        catch (Exception e) {
            Log.Error($"Strategy Simulation Failed: {e}", e);
            Error = e.ToString();
        }
        Strategy.Save();
        FinishedAt = DateTimeOffset.UtcNow;
        State = SimulationState.Finished;
    }
}