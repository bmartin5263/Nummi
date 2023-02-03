namespace Nummi.Core.Domain.Crypto.Bots; 

public class BotNotReadyException : Exception {
    public TimeSpan WaitTime { get; }

    public BotNotReadyException(TimeSpan waitTime) {
        WaitTime = waitTime;
    }
}