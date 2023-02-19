using Nummi.Core.Database.Common;

namespace Nummi.Core.Domain.New.Queries; 

public class GetSimulationsQuery {
    private ISimulationRepository SimulationRepository { get; }

    public GetSimulationsQuery(ISimulationRepository simulationRepository) {
        SimulationRepository = simulationRepository;
    }

    public IEnumerable<Simulation> Execute() {
        return SimulationRepository
            .FindAll()
            .Select(v => {
                SimulationRepository.LoadProperty(v, e => e.Strategy);
                return v;
            });
    }
    
}