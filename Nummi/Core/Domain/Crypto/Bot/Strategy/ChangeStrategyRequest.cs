namespace Nummi.Core.Domain.Crypto.Bot.Strategy; 

public class ChangeStrategyRequest {
    public string StrategyName { get; set; }
    public string FullStrategyName => GetType().Namespace + '.' + StrategyName;
}