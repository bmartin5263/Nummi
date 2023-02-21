using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.New.User;

namespace Nummi.Core.Database.Common; 

public interface IUserRepository : IGenericRepository<Ksuid, NummiUser> {
}