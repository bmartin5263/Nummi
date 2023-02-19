using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.New;

namespace Nummi.Core.Database.EFCore; 

public class SimulationRepository : GenericRepository<Ksuid, Simulation>, ISimulationRepository {
    public SimulationRepository(ITransaction context) : base(context) { }
}