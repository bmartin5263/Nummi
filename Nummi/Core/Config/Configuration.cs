using Nummi.Core.Domain.User;
using Nummi.Core.Exceptions;

namespace Nummi.Core.Config; 

public static class Configuration {
    public static readonly IdentityId ADMIN_USER_ID = IdentityId.FromString("52f12441f3874d38835bdcc3174b9f1a");
    public static readonly IdentityId ROLE_ADMIN_ID = IdentityId.FromString("480264c84e1d4f84be55cc266c5a837c");
    public static readonly IdentityId ROLE_USER_ID = IdentityId.FromString("87fa33c8473a49da8c8b01edff6e1e53");
    
    public const string ADMIN_USER_PASSWORD_ENV_VAR = "NUMMI_ADMIN_PASSWORD";
    public const string ADMIN_USER_EMAIL = "admin@example.com";
    public const string ALPACA_PAPER_ID_ENV_VAR = "NUMMI_ALPACA_PAPER_ID";
    public const string ALPACA_PAPER_KEY_ENV_VAR = "NUMMI_ALPACA_PAPER_KEY";
    public const string ALPACA_LIVE_ID_ENV_VAR = "NUMMI_ALPACA_LIVE_ID";
    public const string ALPACA_LIVE_KEY_ENV_VAR = "NUMMI_ALPACA_LIVE_KEY";

    public static string GetEnvVar(string name) {
        return Environment.GetEnvironmentVariable(name) 
               ?? throw new SystemArgumentException($"Missing Env Var: {name}");
    }
}