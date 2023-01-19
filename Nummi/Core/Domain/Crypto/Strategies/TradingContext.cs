using Nummi.Core.Database;
using Nummi.Core.Domain.Crypto.Client;
using Nummi.Core.External.Binance;

namespace Nummi.Core.Domain.Crypto.Strategies;

public class TradingContext {
    public TradingEnvironment Environment { get; }
    public ICryptoClient CryptoClient { get; }
    public decimal Allowance { get; }
    public BinanceClientAdapter BinanceClientAdapter { get; }
    public AppDb AppDb { get; }
    
    public TradingContext(
        TradingEnvironment environment, 
        ICryptoClient cryptoClient, 
        decimal allowance, 
        BinanceClientAdapter binanceClientAdapter, 
        AppDb appDb
    ) {
        Environment = environment;
        CryptoClient = cryptoClient;
        Allowance = allowance;
        BinanceClientAdapter = binanceClientAdapter;
        AppDb = appDb;
    }
}