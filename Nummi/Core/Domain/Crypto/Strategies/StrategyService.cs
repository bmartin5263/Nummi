using System.Text.Json.Nodes;
using Nummi.Core.Database;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Strategies; 

public class StrategyService {
    
    private AppDb AppDb { get; }

    public StrategyService(AppDb appDb) {
        AppDb = appDb;
    }

    public Strategy CreateStrategy(string name, JsonNode? parameterObject) {
        var strategy = StrategyFactory.Create(name, parameterObject);
        AppDb.Strategies.Add(strategy);
        AppDb.SaveChanges();
        return strategy;
    }

    public Strategy UpdateStrategyParameters(string id, JsonNode parameterObject) {
        var strategy = AppDb.Strategies.FindById(id);
        StrategyFactory.InjectParameterObject(strategy, parameterObject);
        AppDb.SaveChanges();
        return strategy;
    }
    
    public IEnumerable<Strategy> GetStrategies() {
        return AppDb.Strategies.ToList();
    }
    
    public Strategy GetStrategyById(string id) {
        var strategy = AppDb.Strategies
            .FirstOrDefault(b => b.Id == id);

        if (strategy == null) {
            throw new EntityNotFoundException<Strategy>(id);
        }
        
        return strategy;
    }

    public void ClearErrorState(string strategyId) {
        var strategy = GetStrategyById(strategyId);
        strategy.ClearErrorState();
        AppDb.Strategies.Update(strategy);
        AppDb.SaveChanges();
    }
}