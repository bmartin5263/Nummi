using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using KSUID;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Strategies; 

[PrimaryKey(nameof(Id))]
[Table(nameof(StrategyLog))]
[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Persisted precomputed values")]
public class StrategyLog {
    
    public string Id { get; } = Ksuid.Generate().ToString();

    [JsonIgnore]
    public Strategy Strategy { get; private set; } = default!;
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TradingMode Mode { get; private set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StrategyAction Action { get; private set; }

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

    public StrategyLog(Strategy strategy, TradingMode mode, StrategyAction action, DateTime startTime, DateTime endTime, string? error = null) {
        Strategy = strategy;
        Mode = mode;
        StartTime = startTime;
        EndTime = endTime;
        Error = error;
    }

    public override string ToString() {
        return this.ToFormattedString();
    }
}

public enum StrategyAction {
    Initializing, Trading
}