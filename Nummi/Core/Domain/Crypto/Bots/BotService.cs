using System.Net;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Database;
using Nummi.Core.Domain.Crypto.Bots.Thread;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;
using static Nummi.Core.Util.Assertions;

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
        Assert(AppDb.SaveChanges() == 1);
        return bot;
    }

    public BotThreadDetail ActivateBot(string id) {
        var bot = GetBotById(id);
        return ExecutionManager.AssignBot(bot);
    }
    
    public BotThreadDetail DeactivateBot(string id) {
        var bot = GetBotById(id);
        return ExecutionManager.RemoveBot(bot);
    }

    public Bot GetBotById(string id) {
        return AppDb.Bots
            .Include(b => b.Strategy)
            .FirstOrDefault(b => b.Id == id)
            .OrElseThrow(() => new EntityNotFoundException(typeof(Bot), id, HttpStatusCode.BadRequest));
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
        var bot = GetBotById(botId);
        var strategy = AppDb.Strategies.GetById(strategyId, HttpStatusCode.BadRequest);
        bot.Strategy = strategy;
        AppDb.SaveChanges();
        return GetBotById(botId);
    }

    public void ClearErrorState(string botId) {
        var bot = GetBotById(botId);
        bot.ClearErrorState();
        AppDb.SaveChanges();
    }

    public string RunBotSimulation(string botId, SimulationParameters parameters) {
        var bot = GetBotById(botId);
        var result = new SimulationResult();
        ExecutionManager.RunBotSimulation(bot, parameters, result);

        AppDb.SimulationResults.Add(result);
        AppDb.SaveChanges();
        
        return result.Id;
    }

    public SimulationResult GetSimulationResult(string simulationId) {
        return AppDb.SimulationResults
            .Find(simulationId)
            .OrElseThrow(() => new EntityNotFoundException(typeof(SimulationResult), simulationId, HttpStatusCode.BadRequest));
    }
}