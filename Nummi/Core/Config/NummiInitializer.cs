using Microsoft.AspNetCore.Identity;
using NLog;
using Nummi.Core.App.Strategies;
using Nummi.Core.Bridge;
using Nummi.Core.Domain.User;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;
using static Nummi.Core.Config.Configuration;

namespace Nummi.Core.Config; 

/// <summary>
/// Initializes the application with its default data, including creating an admin user,
/// identity roles, and some initial strategies
/// </summary>
public class NummiInitializer : BackgroundService {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private INummiServiceProvider ServiceProvider { get; }
    
    public NummiInitializer(INummiServiceProvider services){
        ServiceProvider = services;
    }

    private bool IsInitialized(INummiUserManager userManager) {
        return userManager.RoleExists(RoleName.User.ToString());
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        Log.Info("Running Nummi Initializer");
        using var scope = ServiceProvider.CreateScope();
        var userManager = scope.GetScoped<INummiUserManager>();
        
        if (IsInitialized(userManager)) {
            Log.Info("Skipping Initialization");
            return;
        }
        
        Log.Info($"{"Creating".Green()} Identity Role {RoleName.User.ToString().Yellow()}");

        var userRole = new NummiRole {
            Id = ROLE_USER_ID,
            Name = RoleName.User.ToString()
        };
        AssertNotFailed(await userManager.CreateRoleAsync(userRole));
        
        Log.Info($"{"Creating".Green()} Identity Role {RoleName.Admin.ToString().Yellow()}");

        var adminRole = new NummiRole {
            Id = ROLE_ADMIN_ID,
            Name = RoleName.Admin.ToString()
        };
        AssertNotFailed(await userManager.CreateRoleAsync(adminRole));

        Log.Info($"{"Creating".Green()} Admin User");

        var admin = new NummiUser {
            Id = ADMIN_USER_ID,
            CreatedAt = DateTimeOffset.UtcNow,
            UserName = "admin",
            Email = ADMIN_USER_EMAIL,
            EmailConfirmed = true,
            SecurityStamp = string.Empty,
            AlpacaPaperId = GetEnvVar(ALPACA_PAPER_ID_ENV_VAR),
            AlpacaPaperKey = GetEnvVar(ALPACA_PAPER_KEY_ENV_VAR),
            AlpacaLiveId = GetEnvVar(ALPACA_LIVE_ID_ENV_VAR),
            AlpacaLiveKey = GetEnvVar(ALPACA_LIVE_KEY_ENV_VAR)
        };
        AssertNotFailed(await userManager.CreateUserAsync(admin, GetEnvVar(ADMIN_PASSWORD_ENV_VAR)));
        AssertNotFailed(await userManager.AssignRoleAsync(admin, RoleName.Admin.ToString()));

        var initializeStrategiesCommand = scope.GetScoped<InitializeBuiltinStrategiesCommand>();
        initializeStrategiesCommand.Execute();
        
        Log.Info("Initialization Complete");
    }

    private void AssertNotFailed(IdentityResult result) {
        if (!result.Succeeded) {
            // TODO - test ToString()
            throw new InvalidSystemConfigurationException(result.Errors.ToString()!);
        }
    }
}