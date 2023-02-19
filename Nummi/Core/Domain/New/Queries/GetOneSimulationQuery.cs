using Nummi.Core.Database.Common;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New.Queries; 

public class GetOneSimulationQuery {
    private ISimulationRepository SimulationRepository { get; }

    public GetOneSimulationQuery(ISimulationRepository simulationRepository) {
        SimulationRepository = simulationRepository;
    }

    public Simulation Execute(string id) {
        var simulation = SimulationRepository.FindById(id.ToKsuid());
        SimulationRepository.LoadProperty(simulation, s => s.Strategy);
        return simulation;
    }
    
}