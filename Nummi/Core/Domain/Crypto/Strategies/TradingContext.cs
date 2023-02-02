using Nummi.Core.Database;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto.Client;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Strategies;

public class TradingContext : ITradingContext {
    public TradingMode Mode { get; }
    public ICryptoDataClient DataClient { get; }
    public ICryptoTradingClient TradingClient { get; }
    public decimal Allowance { get; }
    public AppDb AppDb { get; }
    public IClock Clock { get; }
    
    public TradingContext(
        TradingMode mode, 
        ICryptoDataClient dataClient, 
        ICryptoTradingClient tradingClient,
        decimal allowance, 
        AppDb appDb, 
        IClock clock
    ) {
        Mode = mode;
        DataClient = dataClient;
        TradingClient = tradingClient;
        Allowance = allowance;
        AppDb = appDb;
        Clock = clock;
    }

    public IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period) {
        return DataClient.GetBars(symbols, dateRange, period);
    }
}