using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using KSUID;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Strategies; 

[PrimaryKey(nameof(Id))]
[Table(nameof(StrategyLog))]
[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Persisted precomputed values")]
public class StrategyLog {
    
    public string Id { get; } = Ksuid.Generate().ToString();

    public virtual Strategy Strategy { get; private set; } = default!;
    
    public TradingEnvironment Environment { get; private set; }

    public DateTime StartTime { get; private set; }
    
    public DateTime EndTime { get; private set; }

    [Column(nameof(TotalTime))]
    [SuppressMessage("ReSharper", "ValueParameterNotUsed", Justification = "Needed for Get-only properties")]
    public TimeSpan TotalTime {
        get => EndTime - StartTime;
        private set { }
    }

    public string? Error { get; private set; }

    [SuppressMessage("ReSharper", "UnusedMember.Local", Justification = "Used during Json conversion")]
    private StrategyLog() {}

    public StrategyLog(Strategy strategy, TradingEnvironment environment, DateTime startTime, DateTime endTime, string? error = null) {
        Strategy = strategy;
        Environment = environment;
        StartTime = startTime;
        EndTime = endTime;
        Error = error;
    }

    public override string ToString() {
        return this.ToFormattedString();
    }
}