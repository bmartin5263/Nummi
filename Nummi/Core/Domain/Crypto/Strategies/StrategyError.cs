namespace Nummi.Core.Domain.Crypto.Strategies; 

public class StrategyError {
    public DateTime OccurredAt { get; }
    public string Message { get; }

    public StrategyError(DateTime occurredAt, string message) {
        OccurredAt = occurredAt;
        Message = message;
    }
}