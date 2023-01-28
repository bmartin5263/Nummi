using Nummi.Core.Database;
using Nummi.Core.Domain.Crypto.Client;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Strategies;

public class TradingEnvironment {
    public TradingMode Mode { get; }
    public ICryptoDataClient DataClient { get; }
    public decimal Allowance { get; }
    public AppDb AppDb { get; }
    public IClock Clock { get; }
    
    public TradingEnvironment(
        TradingMode mode, 
        ICryptoDataClient dataClient, 
        decimal allowance, 
        AppDb appDb, 
        IClock clock
    ) {
        Mode = mode;
        DataClient = dataClient;
        Allowance = allowance;
        AppDb = appDb;
        Clock = clock;
    }
}