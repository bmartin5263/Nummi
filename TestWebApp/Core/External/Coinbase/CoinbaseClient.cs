using Coinbase;
using Coinbase.Models;

namespace TestWebApp.Core.External.Coinbase; 

public class CoinbaseClient {
    
    public async Task<Response<Money>> GetSpotPriceAsync(string symbol) {
        return await Client.Data.GetSpotPriceAsync(symbol);
    }

    private global::Coinbase.CoinbaseClient Client { get; } = new(new ApiKeyConfig {
        ApiKey = Environment.GetEnvironmentVariable("COINBASE_KEY") ?? throw new ArgumentException("Missing COINBASE_KEY"), 
        ApiSecret = Environment.GetEnvironmentVariable("COINBASE_SECRET") ?? throw new ArgumentException("Missing COINBASE_SECRET")
    });
    
}