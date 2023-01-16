namespace Nummi.Core.Domain.Crypto.Strategies; 

public class StrategyErrorHistory {
    public List<StrategyError> History { get; } = new();
    public void Add(StrategyError error) {
        History.Add(error);
    }
}