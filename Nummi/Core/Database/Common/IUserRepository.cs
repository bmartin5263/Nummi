using Nummi.Core.Domain.New;

namespace Nummi.Core.Database.Common; 

public interface IUserRepository : IGenericRepository<string, NummiUser> {
}