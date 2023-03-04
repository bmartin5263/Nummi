namespace Nummi.Core.Domain.Strategies; 

public record StrategyExecutionResult {
    public IEnumerable<StrategyLog> Logs { get; }
    public Exception? Error { get; }
    public bool Failed => Error != null;

    private StrategyExecutionResult(IEnumerable<StrategyLog> logs, Exception? error = null) {
        Logs = logs;
        Error = error;
    }

    public static StrategyExecutionResult Success(IEnumerable<StrategyLog> logs) {
        return new StrategyExecutionResult(logs);
    }
    
    public static StrategyExecutionResult Fail(IEnumerable<StrategyLog> logs, Exception error) {
        return new StrategyExecutionResult(logs, error);
    }
}