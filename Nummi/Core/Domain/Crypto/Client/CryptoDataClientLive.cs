using NLog;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.External.Binance;

namespace Nummi.Core.Domain.Crypto.Client; 

public class CryptoDataClientLive : ICryptoDataClient {
    
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private BinanceClient BinanceClient { get; }

    public CryptoDataClientLive(BinanceClient binanceClient) {
        BinanceClient = binanceClient;
    }

    public IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period) {
        return BinanceClient.GetBars(symbols, dateRange, period);
    }
}