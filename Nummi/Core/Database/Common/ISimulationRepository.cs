using Nummi.Core.Domain.Simulations;

namespace Nummi.Core.Database.Common; 

public interface ISimulationRepository : IGenericRepository<SimulationId, Simulation> {
    
}