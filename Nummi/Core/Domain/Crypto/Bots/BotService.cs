using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Database;
using Nummi.Core.Domain.Crypto.Bots.Thread;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Bots; 

public class BotService {

    private AppDb AppDb { get; }
    private BotExecutionManager ExecutionManager { get; }
    private IServiceProvider ServiceProvider { get; }

    public BotService(AppDb appDb, BotExecutionManager executionManager, IServiceProvider serviceProvider) {
        AppDb = appDb;
        ExecutionManager = executionManager;
        ServiceProvider = serviceProvider;
    }

    public Bot CreateBot(CreateBotRequest request) {
        var bot = new Bot(request.Name, request.Funds ?? 0, request.Mode);
        AppDb.Bots.Add(bot);
        Debug.Assert(AppDb.SaveChanges() == 1);
        return bot;
    }

    public BotThreadDetail ActivateBot(string id) {
        var bot = GetBotById(id).OrElseThrow(() => new EntityNotFoundException<Bot>(id));
        return ExecutionManager.AssignBot(bot);
    }
    
    public BotThreadDetail DeactivateBot(string id) {
        var bot = GetBotById(id).OrElseThrow(() => new EntityNotFoundException<Bot>(id));
        return ExecutionManager.RemoveBot(bot);
    }

    public Bot? GetBotById(string id) {
        return AppDb.Bots
            .Include(b => b.Strategy)
            .FirstOrDefault(b => b.Id == id);
    }

    public void DeleteBotById(string id) {
        var bot = AppDb.Bots.Find(id);
        if (bot == null) {
            return;
        }
        AppDb.Bots.Remove(bot);
        AppDb.SaveChanges();
    }

    public IEnumerable<Bot> GetBots() {
        return AppDb.Bots
            .Include(b => b.Strategy)
            .ToList();
    }

    public Bot SetBotStrategy(string botId, string strategyId) {
        var bot = GetBotById(botId).OrElseThrow(() => new EntityNotFoundException<Bot>(botId));
        var strategy = AppDb.Strategies.GetById(strategyId);
        bot.Strategy = strategy;
        AppDb.SaveChanges();
        return bot;
    }

    public void ClearErrorState(string botId) {
        var bot = GetBotById(botId).OrElseThrow(() => new EntityNotFoundException<Bot>(botId));
        bot.ClearErrorState();
        AppDb.SaveChanges();
    }

    public string RunBotSimulation(string botId, SimulationParameters parameters) {
        var bot = GetBotById(botId).OrElseThrow(() => new EntityNotFoundException<Bot>(botId));
        var result = new Simulation();
        ExecutionManager.RunBotSimulation(bot, parameters, result);

        AppDb.Simulations.Add(result);
        AppDb.SaveChanges();
        
        return result.Id;
    }

    public Simulation GetSimulation(string simulationId) {
        return AppDb.Simulations
            .Find(simulationId)
            .OrElseThrow(() => new EntityNotFoundException<Simulation>(simulationId));
    }
}