using System.Diagnostics.CodeAnalysis;

namespace Nummi.Core.Domain.Crypto.Strategies; 

[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Persisted precomputed values")]
public class StrategyLogBuilder {
    
    public Strategy Strategy { get; private set; }
    public TradingEnvironment Environment { get; private set; }
    public DateTime StartTime { get; private set; }
    public Exception? Error { get; set; }
    
    public StrategyLogBuilder(Strategy strategy, TradingEnvironment environment) {
        Strategy = strategy;
        Environment = environment;
        StartTime = DateTime.UtcNow;
    }

    public StrategyLog Build() {
        return new StrategyLog(
            strategy: Strategy,
            environment: Environment,
            startTime: StartTime,
            endTime: DateTime.UtcNow,
            error: Error?.ToString()
        );
    }
}