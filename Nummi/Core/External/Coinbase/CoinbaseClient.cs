using Coinbase;
using Coinbase.Models;

namespace Nummi.Core.External.Coinbase; 

public class CoinbaseClient {
    
    public async Task<Response<Money>> GetSpotPriceAsync(string symbol) {
        return await Client.Data.GetSpotPriceAsync(symbol);
    }

    private global::Coinbase.CoinbaseClient Client { get; } = new(new ApiKeyConfig {
        ApiKey = Environment.GetEnvironmentVariable("NUMMI_COINBASE_ID") ?? throw new ArgumentException("Missing NUMMI_COINBASE_ID"), 
        ApiSecret = Environment.GetEnvironmentVariable("NUMMI_COINBASE_KEYT") ?? throw new ArgumentException("Missing NUMMI_COINBASE_KEYT")
    });
    
}