using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Simulations;

namespace Nummi.Core.Database.EFCore; 

public class SimulationRepository : GenericRepository<SimulationId, Simulation>, ISimulationRepository {
    public SimulationRepository(ITransaction context) : base(context) { }
}