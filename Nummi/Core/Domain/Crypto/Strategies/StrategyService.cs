// using System.Text.Json.Nodes;
// using Microsoft.EntityFrameworkCore;
// using Nummi.Core.Database;
// using Nummi.Core.Database.EFCore;
// using Nummi.Core.Domain.Crypto.Bots;
// using Nummi.Core.Domain.Crypto.Bots.Thread;
// using Nummi.Core.Exceptions;
// using Nummi.Core.Util;
//
// namespace Nummi.Core.Domain.Crypto.Strategies; 
//
// public class StrategyService {
//     
//     private EFCoreContext AppDb { get; }
//     private BotExecutionManager ExecutionManager { get; }
//
//     public StrategyService(EFCoreContext appDb, BotExecutionManager executionManager) {
//         AppDb = appDb;
//         ExecutionManager = executionManager;
//     }
//
//     public Strategy CreateStrategy(string name, JsonNode? parameterObject) {
//         var strategy = StrategyFactory.Create(name, parameterObject);
//         AppDb.Strategies.Add(strategy);
//         AppDb.SaveChanges();
//         return strategy;
//     }
//
//     public Strategy UpdateStrategyParameters(string id, JsonNode parameterObject) {
//         var strategy = AppDb.Strategies.GetById(id);
//         StrategyFactory.InjectParameterObject(strategy, parameterObject);
//         AppDb.SaveChanges();
//         return strategy;
//     }
//     
//     public IEnumerable<Strategy> GetStrategies() {
//         return AppDb.Strategies.ToList();
//     }
//     
//     public Strategy GetStrategyById(string id) {
//         return AppDb.Strategies.GetById(id);
//     }
//     
//     public string RunBotSimulation(string strategyId, SimulationParameters parameters) {
//         var strategy = GetStrategyById(strategyId).OrElseThrow(() => EntityNotFoundException<Strategy>.IdNotFound(strategyId));
//         var simulation = new Simulation(strategy);
//         strategy.Simulations.Add(simulation);
//         
//         // ExecutionManager.RunBotSimulation(bot, parameters, simulation);
//
//         AppDb.SaveChanges();
//         
//         return simulation.Id;
//     }
// }