namespace Nummi.Core.Domain.Stocks.Bot.Strategy; 

public class ChangeStrategyRequest {
    public string StrategyName { get; set; }
    public string FullStrategyName => GetType().Namespace + '.' + StrategyName;
}