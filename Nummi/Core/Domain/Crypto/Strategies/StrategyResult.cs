namespace Nummi.Core.Domain.Crypto.Strategies; 

public class StrategyResult {
    public TimeSpan SleepTime { get; }
    public StrategyLog? Log { get; }

    public StrategyResult(TimeSpan sleepTime, StrategyLog? log = null) {
        SleepTime = sleepTime;
        Log = log;
    }
}