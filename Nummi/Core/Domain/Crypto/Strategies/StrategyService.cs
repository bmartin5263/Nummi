using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Database;
using Nummi.Core.Domain.Crypto.Strategies.Log;
using Nummi.Core.Exceptions;
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
        var strategy = AppDb.Strategies.GetById(id);
        StrategyFactory.InjectParameterObject(strategy, parameterObject);
        AppDb.SaveChanges();
        return strategy;
    }
    
    public IEnumerable<Strategy> GetStrategies() {
        return AppDb.Strategies.ToList();
    }
    
    public Strategy GetStrategyById(string id) {
        return AppDb.Strategies.GetById(id);
    }
    
    public StrategyLog GetLogById(string id) {
        return AppDb.StrategyLogs
            .Include(l => l.Strategy)
            .FirstOrDefault(l => l.Id == id)
            .OrElseThrow(() => EntityNotFoundException<StrategyLog>.IdNotFound(id));
    }
}