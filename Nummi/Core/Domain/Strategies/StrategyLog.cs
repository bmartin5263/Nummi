using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Nummi.Core.Domain.Bots;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Strategies; 

public enum StrategyAction {
    Initializing, Trading
}

public readonly record struct StrategyLogId(Guid Value) {
    public override string ToString() => Value.ToString("N");
    public static StrategyLogId Generate() => new(Guid.NewGuid());
    public static StrategyLogId FromGuid(Guid id) => new(id);
    public static StrategyLogId FromString(string s) => new(Guid.ParseExact(s, "N"));
}

public class StrategyLog : IComparable<StrategyLog> {
    
    public StrategyLogId Id { get; } = StrategyLogId.Generate();
    
    public BotLogId? BotLogId { get; init; }

    public required TradingMode Mode { get; init; }
    
    public required StrategyAction Action { get; init; }

    public required DateTimeOffset StartTime { get; init; }
    
    public required DateTimeOffset EndTime { get; init; }

    [SuppressMessage("ReSharper", "ValueParameterNotUsed", Justification = "Needed for Get-only properties")]
    public TimeSpan TotalTime {
        get => EndTime - StartTime;
        private init { }
    }
    
    public required int ApiCalls { get; init; }
    
    public required TimeSpan TotalApiCallTime { get; init; }

    public string? Error { get; init; }
    
    [NotMapped]
    public Exception? Exception { get; init; }

    public IList<OrderLog> Orders { get; init; } = new List<OrderLog>();

    public int CompareTo(StrategyLog? other) {
        return StartTime.CompareTo(other!.StartTime);
    }

    public override string ToString() {
        return this.ToFormattedString();
    }
}