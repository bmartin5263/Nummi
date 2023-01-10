namespace Nummi.Core.Domain.Crypto.Bot; 

public class BotError {
    public DateTime OccurredAt { get; private set; }
    public string Message { get; private set; }

    public BotError(DateTime occurredAt, string message) {
        OccurredAt = occurredAt;
        Message = message;
    }
}