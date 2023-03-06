using Microsoft.EntityFrameworkCore;
using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.User;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Database.EFCore; 

public class UserRepository : GenericRepository<IdentityId, NummiUser>, IUserRepository {
    public UserRepository(ITransaction context) : base(context) { }
    
    public NummiUser FindByIdWithAllDetails(IdentityId id) {
        var entity = Context.Users
            .Include(u => u.Bots).ThenInclude(b => b.CurrentActivation)
            .Include(u => u.Bots).ThenInclude(b => b.ActivationHistory)
            .Include(u => u.Simulations).ThenInclude(s => s.Strategy)
            .Include(u => u.StrategyTemplates).ThenInclude(t => t.Versions)
            .FirstOrDefault(u => u.Id == id)
            .OrElseThrow(() => EntityNotFoundException<NummiUser>.IdNotFound(id));

        if (!typeof(Audited).IsAssignableFrom(typeof(NummiUser))) {
            return entity;
        }
        
        var audited = (entity as Audited)!;
        if (audited.IsDeleted) {
            throw EntityNotFoundException<NummiUser>.IdNotFound(id);
        }

        return entity;
    }

    public bool ExistsByUsername(string username) {
        return Context.Users.Any(u => u.NormalizedUserName == username.ToUpper());
    }

    public bool ExistsByEmail(string email) {
        return Context.Users.Any(u => u.NormalizedEmail == email.ToUpper());
    }
}