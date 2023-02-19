using Nummi.Core.Domain.Common;

namespace Nummi.Core.Database.Common; 

public interface IAuditedGenericRepository<ID, E> : IGenericRepository<ID, E> where E : class, Audited {
    
}