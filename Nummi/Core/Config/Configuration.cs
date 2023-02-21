using Nummi.Core.Domain.Common;

namespace Nummi.Core.Config; 

public static class Configuration {
    public static readonly Ksuid ADMIN_USER_ID = Ksuid.FromString("2M1NBndHUGhr46XCJVeGsF4syxx");
    // public const string ADMIN_USER_NAME = "Admin";
    public const string ADMIN_USER_PASSWORD_ENV_VAR = "NUMMI_ADMIN_PASSWORD";
    public const string ADMIN_USER_EMAIL = "admin@example.com";
    
    public static readonly Ksuid ROLE_ADMIN_ID = Ksuid.FromString("2M1NEqDjAK2xYGoEhzWrf7ezeLj");
    public const string ROLE_ADMIN_NAME = "Admin";
    
    public static readonly Ksuid ROLE_USER_ID = Ksuid.FromString("2M1NFogR9IWPGyOie3UjrAGdjU9");
    public const string ROLE_USER_NAME = "User";

    public const string ALPACA_PAPER_ID_ENV_VAR = "NUMMI_ALPACA_PAPER_ID";
    public const string ALPACA_PAPER_KEY_ENV_VAR = "NUMMI_ALPACA_PAPER_KEY";
    public const string ALPACA_LIVE_ID_ENV_VAR = "NUMMI_ALPACA_LIVE_ID";
    public const string ALPACA_LIVE_KEY_ENV_VAR = "NUMMI_ALPACA_LIVE_KEY";
}