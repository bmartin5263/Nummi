using Nummi.Core.Database;
using Nummi.Core.Domain.Crypto.Client;

namespace Nummi.Core.Domain.Crypto.Strategies;

public class TradingContext {
    public TradingEnvironment Environment { get; }
    public ICryptoClient CryptoClient { get; }
    public decimal Allowance { get; }
    public AppDb AppDb { get; }
    
    public TradingContext(
        TradingEnvironment environment, 
        ICryptoClient cryptoClient, 
        decimal allowance, 
        AppDb appDb
    ) {
        Environment = environment;
        CryptoClient = cryptoClient;
        Allowance = allowance;
        AppDb = appDb;
    }
}