// using Nummi.Core.Database.Common;
// using Nummi.Core.Domain.Crypto.Bots.Thread;
// using Nummi.Core.Domain.Crypto.Strategies;
// using Nummi.Core.Exceptions;
// using Nummi.Core.Util;
//
// namespace Nummi.Core.Domain.Crypto.Bots; 
//
// public class BotService {
//
//     private ITransaction Db { get; }
//     private BotExecutionManager ExecutionManager { get; }
//     private IServiceProvider ServiceProvider { get; }
//
//     public BotService(ITransaction db, BotExecutionManager executionManager, IServiceProvider serviceProvider) {
//         Db = db;
//         ExecutionManager = executionManager;
//         ServiceProvider = serviceProvider;
//     }
//
//     public Bot CreateBot(CreateBotRequest request) {
//         var bot = new Bot(request.Name, request.Funds ?? 0, request.Mode);
//         if (request.StrategyId != null) {
//             var strategy = Db.StrategyRepository.FindById(request.StrategyId);
//             bot.Strategy = strategy;
//         }
//         
//         // Db.BotRepository.Add(bot);
//         Db.SaveChanges();
//         return bot;
//     }
//
//     public BotThreadDetail ActivateBot(string id) {
//         var bot = GetBotById(id).OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(id));
//         return ExecutionManager.AssignBot(bot);
//     }
//     
//     public BotThreadDetail DeactivateBot(string id) {
//         var bot = GetBotById(id).OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(id));
//         return ExecutionManager.RemoveBot(bot);
//     }
//
//     public Bot? GetBotById(string id) {
//         return null;
//     }
//
//     public void DeleteBotById(string id) {
//         var bot = Db.BotRepository.FindById(id);
//         if (bot == null) {
//             return;
//         }
//
//         // if (ExecutionManager.IsBotActive(bot)) {
//         //     throw new InvalidUserArgumentException("Cannot delete active bots");
//         // }
//         Db.BotRepository.Remove(bot);
//         Db.SaveChanges();
//     }
//     //
//     // public IEnumerable<Bot> GetBots() {
//     //     return Db.BotRepository.FindAll();
//     // }
//
//     public Bot SetBotStrategy(string botId, string strategyId) {
//         var bot = GetBotById(botId).OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(botId));
//         var strategy = Db.StrategyRepository.FindById(strategyId).OrElseThrow(() => EntityNotFoundException<Strategy>.IdNotFound(strategyId));
//         bot.Strategy = strategy;
//         Db.SaveChanges();
//         return bot;
//     }
//
//     public void ClearErrorState(string botId) {
//         var bot = GetBotById(botId).OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(botId));
//         bot.ClearErrorState();
//         Db.SaveChanges();
//     }
//
//     public Simulation GetSimulation(string simulationId) {
//         return Db.SimulationRepository
//             .FindById(simulationId)
//             .OrElseThrow(() => EntityNotFoundException<Simulation>.IdNotFound(simulationId));
//     }
//
//
//     // TODO - strategy service (at least)
//     // public StrategyLog RunStrategy(string botId) {
//     //     using var scope = ServiceProvider.CreateScope();
//     //     using var db = scope.ServiceProvider.GetService<Transaction>()!.EnableAutoCommit();
//     //     var bot = GetBotById(botId).OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(botId));
//     //     
//     //     var log = bot.RunRealtime(new BotContext(
//     //         serviceProvider: ServiceProvider,
//     //         scope: scope,
//     //         db: db
//     //     ));
//     //
//     //     return log;
//     // }
//     //
//     // public BotLog InitializeStrategy(string botId, bool force = false) {
//     //     using var scope = ServiceProvider.CreateScope();
//     //     using var db = scope.ServiceProvider.GetService<Transaction>()!.EnableAutoCommit();
//     //     var bot = GetBotById(botId).OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(botId));
//     //
//     //     if (!bot.Strategy!.ShouldInitialize() && !force) {
//     //         throw new InvalidUserArgumentException("Strategy is already initialized");
//     //     }
//     //
//     //     var tradingContextFactory = scope.ServiceProvider.GetService<TradingContextFactory>()!;
//     //     
//     //     return bot.InitializeStrategy(tradingContextFactory);
//     // }
// }