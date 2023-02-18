using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Common;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New.Commands;

public record SimulateStrategyParameters {
    public required DateTimeOffset StartTime { get; init; }
    public required DateTimeOffset EndTime { get; init; }
    public string? StrategyJsonParameters { get; init; }
}

public class SimulateStrategyCommand {
    private ITransaction Transaction { get; }
    
    public SimulateStrategyCommand(ITransaction transaction) {
        Transaction = transaction;
    }

    public Simulation Execute(string userId, Ksuid strategyTemplateId, SimulateStrategyParameters parameters) {
        var user = Transaction.UserRepository.FindById(userId)
            .OrElseThrow(() => EntityNotFoundException<NummiUser>.IdNotFound(userId));
        var strategyTemplate = Transaction.StrategyTemplateRepository.FindById(strategyTemplateId)
            .OrElseThrow(() => EntityNotFoundException<Strategy>.IdNotFound(strategyTemplateId));
        
        var strategy = strategyTemplate.Instantiate(parameters.StrategyJsonParameters);
        var simulation = new Simulation(strategy, parameters.StartTime, parameters.EndTime);
        user.Simulations.Add(simulation);

        Transaction.SaveAndDispose();
        return simulation;
    }
    
}