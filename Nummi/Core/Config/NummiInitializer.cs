using Microsoft.AspNetCore.Identity;
using NLog;
using Nummi.Core.Bridge;
using Nummi.Core.Domain.New;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;
using static Nummi.Core.Config.Configuration;

namespace Nummi.Core.Config; 

public class NummiInitializer : IHostedService {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private INummiServiceProvider ServiceProvider { get; }
    
    public NummiInitializer(INummiServiceProvider services){
        ServiceProvider = services;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken) {
        Log.Info("Running Nummi Initializer");
        using var scope = ServiceProvider.CreateScope();
        var userManager = scope.GetService<INummiUserManager>();
        
        if (!userManager.RoleExists(ROLE_USER_NAME)) {
            Log.Info($"{"Creating".Green()} Identity Role {ROLE_USER_NAME.Blue()}");

            var userRole = new IdentityRole {
                Id = ROLE_USER_ID,
                Name = ROLE_USER_NAME
            };
            AssertNotFailed(await userManager.CreateRoleAsync(userRole));
        }
        
        if (!userManager.RoleExists(ROLE_ADMIN_NAME)) {
            Log.Info($"{"Creating".Green()} Identity Role {ROLE_ADMIN_NAME.Blue()}");

            var adminRole = new IdentityRole {
                Id = ROLE_ADMIN_ID,
                Name = ROLE_ADMIN_NAME
            };
            AssertNotFailed(await userManager.CreateRoleAsync(adminRole));
        }

        if (!userManager.AdminExists()) {
            Log.Info($"{"Creating".Green()} Admin User");

            var admin = new NummiUser {
                Id = ADMIN_USER_ID,
                CreatedAt = DateTimeOffset.UtcNow,
                UserName = ADMIN_USER_NAME,
                NormalizedUserName = ADMIN_USER_NAME,
                Email = ADMIN_USER_EMAIL,
                NormalizedEmail = ADMIN_USER_EMAIL,
                EmailConfirmed = true,
                SecurityStamp = string.Empty,
                AlpacaPaperId = GetEnvironmentVariable(ALPACA_PAPER_ID_ENV_VAR),
                AlpacaPaperKey = GetEnvironmentVariable(ALPACA_PAPER_KEY_ENV_VAR),
                AlpacaLiveId = GetEnvironmentVariable(ALPACA_LIVE_ID_ENV_VAR),
                AlpacaLiveKey = GetEnvironmentVariable(ALPACA_LIVE_KEY_ENV_VAR)
            };
            AssertNotFailed(await userManager.CreateUserAsync(admin, "Password1!"));
            AssertNotFailed(await userManager.AssignRoleAsync(admin, ROLE_ADMIN_NAME));
        }
        Log.Info("Initialization Complete");
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }

    private string GetEnvironmentVariable(string name) {
        return Environment.GetEnvironmentVariable(name) 
               ?? throw new InvalidSystemArgumentException($"Missing Env Var: {name}");
    }

    private void AssertNotFailed(IdentityResult result) {
        if (!result.Succeeded) {
            // TODO - test ToString()
            throw new InvalidSystemConfigurationException(result.Errors.ToString()!);
        }
    }
}