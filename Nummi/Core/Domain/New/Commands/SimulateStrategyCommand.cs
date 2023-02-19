using System.Text.Json;
using NLog;
using Nummi.Core.Database.Common;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New.Commands;

public record SimulateStrategyParameters {
    public required string StrategyTemplateId { get; init; }
    public required DateTimeOffset StartTime { get; init; }
    public required DateTimeOffset EndTime { get; init; }
    public JsonDocument? StrategyJsonParameters { get; init; }
}

public class SimulateStrategyCommand {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    
    private IUserRepository UserRepository { get; }
    private IStrategyTemplateRepository StrategyTemplateRepository { get; }
    
    public SimulateStrategyCommand(IUserRepository userRepository, IStrategyTemplateRepository strategyTemplateRepository) {
        UserRepository = userRepository;
        StrategyTemplateRepository = strategyTemplateRepository;
    }

    public Simulation Execute(string userId, SimulateStrategyParameters parameters) {
        var user = UserRepository.FindById(userId);
        var strategyTemplate = StrategyTemplateRepository.FindById(parameters.StrategyTemplateId.ToKsuid());
        
        var strategy = strategyTemplate.Instantiate(Serializer.DocumentToJson(parameters.StrategyJsonParameters));
        var simulation = new Simulation(strategy, parameters.StartTime, parameters.EndTime);
        user.Simulations.Add(simulation);

        UserRepository.Commit();
        return simulation;
    }
    
}