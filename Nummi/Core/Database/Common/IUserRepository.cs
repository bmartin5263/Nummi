using Nummi.Core.Domain.User;

namespace Nummi.Core.Database.Common; 

public interface IUserRepository : IGenericRepository<IdentityId, NummiUser> {
    NummiUser FindByIdWithAllDetails(IdentityId id);
    bool ExistsByUsername(string username);
    bool ExistsByEmail(string email);
}