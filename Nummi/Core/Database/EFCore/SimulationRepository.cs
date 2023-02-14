using KSUID;
using Nummi.Core.Database.Common;
using Nummi.Core.Domain.New;

namespace Nummi.Core.Database.EFCore; 

public class SimulationRepository : GenericRepository<Ksuid, Simulation>, ISimulationRepository {
    public SimulationRepository(EFCoreContext context) : base(context) { }
}