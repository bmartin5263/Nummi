using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Database;
using Nummi.Core.Domain.Crypto.Bots.Thread;
using Nummi.Core.Domain.Crypto.Strategies;
using Nummi.Core.Domain.Crypto.Strategies.Log;
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
        var bot = GetBotById(id).OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(id));
        return ExecutionManager.AssignBot(bot);
    }
    
    public BotThreadDetail DeactivateBot(string id) {
        var bot = GetBotById(id).OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(id));
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

        if (ExecutionManager.IsBotActive(bot)) {
            throw new InvalidUserArgumentException("Cannot delete active bots");
        }
        AppDb.Bots.Remove(bot);
        AppDb.SaveChanges();
    }

    public IEnumerable<Bot> GetBots() {
        return AppDb.Bots
            .Include(b => b.Strategy)
            .Include(b => b.LastStrategyLog)
            .ToList();
    }

    public Bot SetBotStrategy(string botId, string strategyId) {
        var bot = GetBotById(botId).OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(botId));
        var strategy = AppDb.Strategies.GetById(strategyId);
        bot.Strategy = strategy;
        AppDb.SaveChanges();
        return bot;
    }

    public void ClearErrorState(string botId) {
        var bot = GetBotById(botId).OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(botId));
        bot.ClearErrorState();
        AppDb.SaveChanges();
    }

    public string RunBotSimulation(string botId, SimulationParameters parameters) {
        var bot = GetBotById(botId).OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(botId));
        var simulation = new Simulation(bot.Strategy!);
        bot.Simulations.Add(simulation);
        
        ExecutionManager.RunBotSimulation(bot, parameters, simulation);

        AppDb.SaveChanges();
        
        return simulation.Id;
    }

    public Simulation GetSimulation(string simulationId) {
        return AppDb.Simulations
            .Find(simulationId)
            .OrElseThrow(() => EntityNotFoundException<Simulation>.IdNotFound(simulationId));
    }


    public StrategyLog RunStrategy(string botId) {
        var scope = ServiceProvider.CreateScope();
        var db = scope.ServiceProvider.GetService<AppDb>()!;
        var bot = GetBotById(botId).OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(botId));
        
        var log = bot.RunRealtime(new BotContext(
            serviceProvider: ServiceProvider,
            scope: scope,
            appDb: db
        ));
        db.SaveChanges();
        return log;
    }

    public StrategyLog InitializeStrategy(string botId, bool force = false) {
        var scope = ServiceProvider.CreateScope();
        var db = scope.ServiceProvider.GetService<AppDb>()!;
        var bot = GetBotById(botId).OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(botId));
        var strategy = bot.Strategy.OrElseThrow(() => new EntityNotFoundException<Strategy>("Bot does not have a strategy"));

        if (!strategy.ShouldInitialize() && !force) {
            throw new InvalidUserArgumentException("Strategy is already initialized");
        }

        var tradingContextFactory = scope.ServiceProvider.GetService<TradingContextFactory>()!;
        
        var log = bot.InitializeStrategy(tradingContextFactory);
        db.SaveChanges();
        return log;
    }
}