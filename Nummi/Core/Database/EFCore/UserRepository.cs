using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.New;
using Nummi.Core.Domain.New.User;

namespace Nummi.Core.Database.EFCore; 

public class UserRepository : GenericRepository<Ksuid, NummiUser>, IUserRepository {
    public UserRepository(ITransaction context) : base(context) { }
}