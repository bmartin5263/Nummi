namespace Nummi.Core.Domain.Crypto.Strategies; 


public class StrategyErrorHistory {
    public IList<StrategyError> History { get; set; }

    public StrategyErrorHistory() {
        History = new List<StrategyError>();
    }

    public StrategyErrorHistory(IList<StrategyError> history) {
        History = history;
    }

    public void Add(StrategyError error) {
        History.Add(error);
    }
}