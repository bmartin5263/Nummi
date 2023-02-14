using KSUID;
using Nummi.Core.Domain.New;

namespace Nummi.Core.Database.Common; 

public interface IBotRepository : IGenericRepository<Ksuid, Bot> {
}