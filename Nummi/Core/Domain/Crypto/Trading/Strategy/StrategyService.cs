using System.Text.Json.Nodes;
using Nummi.Core.Database;

namespace Nummi.Core.Domain.Crypto.Trading.Strategy; 

public class StrategyService {
    
    private AppDb AppDb { get; }

    public StrategyService(AppDb appDb) {
        AppDb = appDb;
    }

    public TradingStrategy CreateStrategy(string name, JsonNode? parameterObject) {
        var strategy = StrategyFactory.Create(name, parameterObject);
        AppDb.Strategies.Add(strategy);
        AppDb.SaveChanges();
        return strategy;
    }
    
}