using Microsoft.AspNetCore.Identity;
using Nummi.Core.Database.Common;
using Nummi.Core.Domain.New;
using Nummi.Core.Exceptions;
using static Nummi.Core.Config.Configuration;

namespace Nummi.Core.Bridge.DotNet; 

public class AspDotNetUserManager : INummiUserManager {
    
    private IUserRepository UserRepository { get; }
    private UserManager<NummiUser> UserManager { get; }
    private RoleManager<IdentityRole> RoleManager { get; }

    public AspDotNetUserManager(IUserRepository userRepository, UserManager<NummiUser> userManager, RoleManager<IdentityRole> roleManager) {
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

    public IdentityResult CreateUser(NummiUser user, string password) {
        var result = UserManager.CreateAsync(user, password).Result;
        if (!result.Succeeded) {
            throw new InvalidSystemConfigurationException(result.Errors.ToString()!);
        }
        return result;
    }

    public IdentityResult CreateRole(IdentityRole role) {
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

    public Task<IdentityResult> CreateRoleAsync(IdentityRole role) {
        return RoleManager.CreateAsync(role);
    }

    public Task<IdentityResult> AssignRoleAsync(NummiUser user, string roleName) {
        return UserManager.AddToRoleAsync(user, roleName);
    }
}