using Nummi.Core.Domain.User;

namespace Nummi.Core.Database.Common; 

public interface IUserRepository : IGenericRepository<string, NummiUser> {
}