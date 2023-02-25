using System.Diagnostics.CodeAnalysis;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Strategies; 

public enum StrategyAction {
    Initializing, Trading
}

public class StrategyLog : IComparable<StrategyLog> {
    
    public Ksuid Id { get; } = Ksuid.Generate();
    
    public Ksuid? BotLogId { get; init; }

    public required TradingMode Mode { get; init; }
    
    public required StrategyAction Action { get; init; }

    public required DateTime StartTime { get; init; }
    
    public required DateTime EndTime { get; init; }

    [SuppressMessage("ReSharper", "ValueParameterNotUsed", Justification = "Needed for Get-only properties")]
    public TimeSpan TotalTime {
        get => EndTime - StartTime;
        private init { }
    }
    
    public required int ApiCalls { get; init; }
    
    public required TimeSpan TotalApiCallTime { get; init; }

    public string? Error { get; init; }

    public IList<OrderLog> Orders { get; init; } = new List<OrderLog>();

    public int CompareTo(StrategyLog? other) {
        return StartTime.CompareTo(other?.StartTime);
    }

    public override string ToString() {
        return this.ToFormattedString();
    }
}