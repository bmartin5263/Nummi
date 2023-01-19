using System.Net;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Util;

namespace Nummi.Core.External.Binance; 

public class BinanceClientAdapter {

    private const int IP_BANNED = 419;
    
    private NummiHttpClient Client { get; } = new(new HttpClient(), "https://api.binance.us/api/v3");
    private bool Limited { get; set; }
    
    public IEnumerable<HistoricalPrice> GetSpotPrice(ISet<string> symbols) {
        if (Limited) {
            return new List<HistoricalPrice>();
        }

        return Client.Get("/ticker/price")
            .Parameter("symbols", symbols)
            .Execute()
            .LogHeaders("x-mbx-used-weight", "x-mbx-used-weight-1m", "x-mbx-uuid")
            .OnStatusCode(HttpStatusCode.TooManyRequests, v => LimitThyself(v))
            .OnStatusCode(IP_BANNED, v => LimitThyself(v))
            .ReadJson<IList<BinancePrice>>()
            .Select(v => v.ToHistoricalPrice());
    }
    
    public IEnumerable<MinuteCandlestick> GetMinuteCandlestick(string symbol, DateTime? start = null, uint limit = 1) {
        if (Limited) {
            return new List<MinuteCandlestick>();
        }

        start ??= DateTime.Now.AddMinutes(-1);

        var candlesticks = Client.Get("/klines")
            .Parameter("symbol", symbol)
            .Parameter("limit", limit.ToString())
            .Parameter("interval", "1m")
            .Parameter("startTime", ((DateTimeOffset)start).ToUnixTimeMilliseconds().ToString())
            .Execute()
            .LogHeaders("x-mbx-used-weight", "x-mbx-used-weight-1m", "x-mbx-uuid")
            .OnStatusCode(HttpStatusCode.TooManyRequests, v => LimitThyself(v))
            .OnStatusCode(IP_BANNED, v => LimitThyself(v))
            .ReadJsonElement();

        var result = new List<MinuteCandlestick>();
        var candlesticksIter = candlesticks.EnumerateArray();
        while (candlesticksIter.MoveNext()) {
            var candlestickValues = candlesticksIter.Current.EnumerateArray().ToList();
            result.Add(new MinuteCandlestick(
                symbol: symbol,
                openTimeEpoch: candlestickValues[0].GetInt64(),
                open: decimal.Parse(candlestickValues[1].GetString()!),
                high: decimal.Parse(candlestickValues[2].GetString()!),
                low: decimal.Parse(candlestickValues[3].GetString()!),
                close: decimal.Parse(candlestickValues[4].GetString()!),
                volume: decimal.Parse(candlestickValues[5].GetString()!)
            ));
        }

        return result;
    }

    private void LimitThyself(HttpResponseReader res) {
        Limited = true;
    }

}