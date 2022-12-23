using Alpaca.Markets;
using Environments = Alpaca.Markets.Environments;

namespace TestWebApp.Domain; 

public class AlpacaClient {
    
    // TODO - remove
    private const string KEY_ID = "PK9TAKL8H6B5O5BFCBU2";
    private const string SECRET_KEY = "nyib6BFTQFTAkyYvPMAtzhG3mDmD1gyKatF9sjI0";

    public async Task<IAccount> GetAccountDetails() {
        var client = Environments.Paper
            .GetAlpacaTradingClient(new SecretKey(KEY_ID, SECRET_KEY));
        
        return await client.GetAccountAsync();
    }
    
}