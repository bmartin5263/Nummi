using NLog;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.New;
using Nummi.Core.External.Binance;

namespace Nummi.Core.Domain.Crypto.Data; 

public class CryptoDataClientLive : ICryptoDataClient {
    
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private BinanceClientAdapter BinanceClient { get; }

    public CryptoDataClientLive(BinanceClientAdapter binanceClient) {
        BinanceClient = binanceClient;
    }

    public IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period) {
        return BinanceClient.GetBars(symbols, dateRange, period);
    }
}