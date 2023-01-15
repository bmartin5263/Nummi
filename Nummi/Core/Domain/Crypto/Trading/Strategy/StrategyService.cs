using System.Text.Json.Nodes;
using Nummi.Core.Database;
using Nummi.Core.Util;

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
    
    public IEnumerable<TradingStrategy> GetStrategies() {
        return AppDb.Strategies.ToList();
    }
    
    public TradingStrategy GetStrategyById(string id) {
        var strategy = AppDb.Strategies
            .FirstOrDefault(b => b.Id == id);

        if (strategy == null) {
            throw new EntityNotFoundException<TradingStrategy>(id);
        }
        
        return strategy;
    }
    
}