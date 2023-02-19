using Microsoft.AspNetCore.Identity;
using Nummi.Core.Domain.New;

namespace Nummi.Core.Bridge; 

public interface INummiUserManager {
    public bool AdminExists();
    public bool RoleExists(string roleName);
    public IdentityResult CreateUser(NummiUser user, string password);
    public IdentityResult CreateRole(IdentityRole role);
    public IdentityResult AssignRole(NummiUser user, string roleName);
    public Task<IdentityResult> CreateUserAsync(NummiUser user, string password);
    public Task<IdentityResult> CreateRoleAsync(IdentityRole role);
    public Task<IdentityResult> AssignRoleAsync(NummiUser user, string roleName);
}