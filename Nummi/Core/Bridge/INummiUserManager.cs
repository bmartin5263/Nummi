using Microsoft.AspNetCore.Identity;
using Nummi.Core.Domain.User;

namespace Nummi.Core.Bridge; 

public interface INummiUserManager {
    public bool AdminExists();
    public bool RoleExists(string roleName);
    public bool UserExists(string username);
    public bool EmailExists(string email);
    public Task<IdentityResult> CreateUserAsync(NummiUser user, string password);
    public Task<IdentityResult> CreateRoleAsync(NummiRole role);
    public Task<IdentityResult> AssignRoleAsync(NummiUser user, string roleName);
    
    public Task<NummiUser> LoginAsync(string username, string password);
    public Task<IList<string>> GetRolesAsync(NummiUser user);
}