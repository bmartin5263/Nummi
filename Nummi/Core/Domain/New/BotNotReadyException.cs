namespace Nummi.Core.Domain.New; 

public class BotNotReadyException : Exception {
    public TimeSpan WaitTime { get; }

    public BotNotReadyException(TimeSpan waitTime) {
        WaitTime = waitTime;
    }
}