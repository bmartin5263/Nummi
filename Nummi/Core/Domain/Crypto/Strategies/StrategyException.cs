using Nummi.Core.Domain.Crypto.Strategies.Log;

namespace Nummi.Core.Domain.Crypto.Strategies; 

public class StrategyException : Exception {
    public StrategyLog Log { get; }
    public StrategyException(StrategyLog log) {
        Log = log;
    }
    public StrategyException(StrategyLog log, Exception? innerException) : base("", innerException) {
        Log = log;
    }
}