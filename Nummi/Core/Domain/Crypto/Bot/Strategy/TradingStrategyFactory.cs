namespace Nummi.Core.Domain.Crypto.Bot.Strategy; 

public static class TradingStrategyFactory {
    public static ITradingStrategy Create(string name) {
        Console.WriteLine($"Creating Instance of Strategy {name}");
        Type t = Type.GetType(name)!; 
        var strategy = (ITradingStrategy) Activator.CreateInstance(t)!;
        return strategy;
    }
}