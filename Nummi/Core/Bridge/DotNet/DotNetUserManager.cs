using Microsoft.AspNetCore.Identity;
using Nummi.Core.Database.Common;
using Nummi.Core.Domain.User;
using Nummi.Core.Exceptions;
using static Nummi.Core.Config.Configuration;

namespace Nummi.Core.Bridge.DotNet; 

public class DotNetUserManager : INummiUserManager {
    
    private IUserRepository UserRepository { get; }
    private UserManager<NummiUser> UserManager { get; }
    private RoleManager<NummiRole> RoleManager { get; }

    public DotNetUserManager(IUserRepository userRepository, UserManager<NummiUser> userManager, RoleManager<NummiRole> roleManager) {
        UserRepository = userRepository;
        UserManager = userManager;
        RoleManager = roleManager;
    }

    public bool AdminExists() {
        return UserRepository.ExistsById(ADMIN_USER_ID);
    }

    public bool RoleExists(string roleName) {
        return RoleManager.RoleExistsAsync(roleName).Result;
    }

    public bool UserExists(string username) {
        return UserRepository.ExistsByUsername(username);
    }

    public bool EmailExists(string email) {
        return UserRepository.ExistsByEmail(email);
    }

    public IdentityResult CreateUser(NummiUser user, string password) {
        var result = UserManager.CreateAsync(user, password).Result;
        if (!result.Succeeded) {
            throw new InvalidSystemConfigurationException(result.Errors.ToString()!);
        }
        return result;
    }

    public IdentityResult CreateRole(NummiRole role) {
        var result = RoleManager.CreateAsync(role).Result;
        if (!result.Succeeded) {
            throw new InvalidSystemConfigurationException(result.Errors.ToString()!);
        }
        return result;
    }

    public IdentityResult AssignRole(NummiUser user, string roleName) {
        var result = UserManager.AddToRoleAsync(user, roleName).Result;
        if (!result.Succeeded) {
            throw new InvalidSystemConfigurationException(result.Errors.ToString()!);
        }
        return result;
    }

    public Task<IdentityResult> CreateUserAsync(NummiUser user, string password) {
        return UserManager.CreateAsync(user, password);
    }

    public Task<IdentityResult> CreateRoleAsync(NummiRole role) {
        return RoleManager.CreateAsync(role);
    }

    public Task<IdentityResult> AssignRoleAsync(NummiUser user, string roleName) {
        return UserManager.AddToRoleAsync(user, roleName);
    }

    public async Task<NummiUser> LoginAsync(string username, string password) {
        var user = await UserManager.FindByNameAsync(username);
        if (user == null || !await UserManager.CheckPasswordAsync(user, password)) {
            throw new AuthenticationException($"Could not authorize {username}");
        }

        return user;
    }

    public Task<IList<string>> GetRolesAsync(NummiUser user) {
        return UserManager.GetRolesAsync(user);
    }
}