namespace Nummi.Core.Domain.Crypto.Strategies; 

public class StrategyException : Exception {
    public StrategyException(string? message) : base(message) { }
    public StrategyException(string? message, Exception? innerException) : base(message, innerException) { }
}