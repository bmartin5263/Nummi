using System.ComponentModel.DataAnnotations.Schema;
using KSUID;

namespace Nummi.Core.Domain.Crypto.Strategies; 

[Table(nameof(StrategyExecutionLog))]
public class StrategyExecutionLog {
    
    public string Id { get; } = Ksuid.Generate().ToString();

    public DateTime StartTime { get; }
    
    public DateTime EndTime { get; }

    public TimeSpan TotalTime => EndTime - StartTime;
    
    public string? Error { get; }

    private StrategyExecutionLog() {}

    public StrategyExecutionLog(DateTime startTime, DateTime endTime, string? error = null) {
        StartTime = startTime;
        EndTime = endTime;
        Error = error;
    }
}