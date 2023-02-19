namespace Nummi.Core.Config; 

public static class Configuration {
    public const string ADMIN_USER_ID = "f99f1bb5-b48c-41b1-813f-fe2723248e48";
    public const string ADMIN_USER_NAME = "Admin";
    public const string ADMIN_USER_PASSWORD_ENV_VAR = "NUMMI_ADMIN_PASSWORD";
    public const string ADMIN_USER_EMAIL = "admin@example.com";
    
    public const string ROLE_ADMIN_ID = "f5ec4969-cac0-43e7-9b71-d7851fc63c10";
    public const string ROLE_ADMIN_NAME = "Admin";
    
    public const string ROLE_USER_ID = "dd3e10f5-734d-4697-aed2-39f96ad6b50d";
    public const string ROLE_USER_NAME = "User";

    public const string ALPACA_PAPER_ID_ENV_VAR = "NUMMI_ALPACA_PAPER_ID";
    public const string ALPACA_PAPER_KEY_ENV_VAR = "NUMMI_ALPACA_PAPER_KEY";
    public const string ALPACA_LIVE_ID_ENV_VAR = "NUMMI_ALPACA_LIVE_ID";
    public const string ALPACA_LIVE_KEY_ENV_VAR = "NUMMI_ALPACA_LIVE_KEY";
}