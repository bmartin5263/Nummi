using System.Text.Json;
using NLog;
using Nummi.Core.App.Strategies;
using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.Domain.Simulations;
using Nummi.Core.Domain.Strategies;
using Nummi.Core.Domain.User;
using Nummi.Core.Util;

namespace Nummi.Core.App.Simulations;

public record SimulateStrategyParameters {
    public required IdentityId UserId { get; init; }
    public required StrategyTemplateId StrategyTemplateId { get; init; }
    public required DateTimeOffset StartTime { get; init; }
    public required DateTimeOffset EndTime { get; init; }
    public required decimal Funds { get; init; }
    public JsonDocument? StrategyJsonParameters { get; init; }
}

public class SimulateStrategyCommand {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    
    private IUserRepository UserRepository { get; }
    private TradingSessionFactory TradingSessionFactory { get; }
    private InstantiateStrategyCommand InstantiateStrategyCommand { get; }
    
    public SimulateStrategyCommand(
        IUserRepository userRepository, 
        TradingSessionFactory tradingSessionFactory, 
        InstantiateStrategyCommand instantiateStrategyTemplateCommand
    ) {
        UserRepository = userRepository;
        TradingSessionFactory = tradingSessionFactory;
        InstantiateStrategyCommand = instantiateStrategyTemplateCommand;
    }

    public Simulation Execute(SimulateStrategyParameters args) {
        NummiUser user = UserRepository.RequireById(args.UserId);
        Strategy strategy = InstantiateStrategyCommand.Execute(new InstantiateStrategyParameters {
            StrategyParameters = args.StrategyJsonParameters,
            StrategyTemplateId = args.StrategyTemplateId
        });

        Simulation simulation = new(strategy: strategy, startDate: args.StartTime, endDate: args.EndTime);
        
        user.Simulations.Add(simulation);

        ITradingSession session = TradingSessionFactory.CreateSimulated(funds: args.Funds, clock: new ClockMock());
        simulation.Start(session);
        
        UserRepository.Commit();
        return simulation;
    }

}