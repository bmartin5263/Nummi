namespace Nummi.Core.Domain.Bots; 

public class BotNotReadyException : Exception {
    public TimeSpan WaitTime { get; }

    public BotNotReadyException(TimeSpan waitTime) {
        WaitTime = waitTime;
    }
}