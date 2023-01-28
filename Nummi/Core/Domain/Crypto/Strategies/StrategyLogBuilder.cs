using System.Diagnostics.CodeAnalysis;

namespace Nummi.Core.Domain.Crypto.Strategies; 

[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Persisted precomputed values")]
public class StrategyLogBuilder {
    
    public Strategy Strategy { get; private set; }
    public TradingMode Mode { get; private set; }
    public StrategyAction Action { get; private set; }
    public DateTime StartTime { get; private set; }
    public Exception? Error { get; set; }
    
    public StrategyLogBuilder(Strategy strategy, TradingMode mode, StrategyAction action) {
        Strategy = strategy;
        Mode = mode;
        Action = action;
        StartTime = DateTime.UtcNow;
    }

    public StrategyLog Build() {
        return new StrategyLog(
            strategy: Strategy,
            mode: Mode,
            action: Action,
            startTime: StartTime,
            endTime: DateTime.UtcNow,
            error: Error?.ToString()
        );
    }
}