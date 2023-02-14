using Nummi.Core.Database.Common;
using Nummi.Core.Domain.User;

namespace Nummi.Core.Database.EFCore; 

public class UserRepository : GenericRepository<string, NummiUser>, IUserRepository {
    public UserRepository(EFCoreContext context) : base(context) { }
}