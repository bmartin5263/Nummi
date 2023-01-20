using Nummi.Core.Database;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.External.Binance;

namespace Nummi.Core.Domain.Crypto.Client; 

public abstract class CryptoClientCommon : ICryptoClient {

    private BinanceClient BinanceClient { get; }
    private AppDb AppDb { get; }

    protected CryptoClientCommon(BinanceClient binanceClient, AppDb appDb) {
        BinanceClient = binanceClient;
        AppDb = appDb;
    }

    public IEnumerable<Price> GetSpotPrice(ISet<string> symbols) {
        var prices = BinanceClient.GetSpotPrice(symbols).ToList();
        AppDb.HistoricalPrices.AddRangeAsync(prices);
        return prices;
    }

    public IDictionary<string, MinuteBar> GetMinuteBars(ISet<string> symbols) {
        var bars = BinanceClient.GetMinuteKlines(symbols);
        AppDb.HistoricalMinuteBars.AddRangeAsync(bars.Values);
        return bars;
    }

    public HistoricalBars GetHistoricalMinuteBars(ISet<string> symbols, DateTime startTime) {
        var bars = BinanceClient.GetMinuteKlines(symbols, startTime);
        AppDb.HistoricalMinuteBars.AddRangeAsync(bars.Bars.SelectMany(v => v.Value));
        return bars;
    }
}
