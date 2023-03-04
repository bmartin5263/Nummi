using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Simulations;

namespace Nummi.Core.App.Simulations; 

public class GetOneSimulationQuery {
    private ISimulationRepository SimulationRepository { get; }

    public GetOneSimulationQuery(ISimulationRepository simulationRepository) {
        SimulationRepository = simulationRepository;
    }

    public Simulation Execute(SimulationId id) {
        var simulation = SimulationRepository.FindById(id);
        SimulationRepository.LoadProperty(simulation, s => s.Strategy);
        return simulation;
    }
    
}