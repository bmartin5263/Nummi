using Microsoft.AspNetCore.Mvc;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.External.Binance;

namespace Nummi.Api.Controllers; 

[Route("api/market")]
[ApiController]
public class MarketDataController : ControllerBase {
    
    private BinanceClientAdapter BinanceClient { get; }

    public MarketDataController(BinanceClientAdapter binanceClient) {
        BinanceClient = binanceClient;
    }
    //
    // [Route("price")]
    // [HttpGet]
    // public IEnumerable<Price> GetPrice([FromQuery] HashSet<string> symbols) {
    //     var response = BinanceClient.GetSpotPrice(symbols, DateTime.UtcNow);
    //     return response;
    // }

    [Route("bars")]
    [HttpGet]
    public IDictionary<string, Bar> GetBars([FromQuery] string symbol) {
        var response = BinanceClient.GetBar(
            symbols: new HashSet<string> {symbol},
            time: DateTime.UtcNow,
            period: Period.Minute
        );
        return response;
    }
    
    [Route("exchange-info")]
    [HttpGet]
    public ExchangeInfo GetExchangeInfo() {
        var response = BinanceClient.GetExchangeInfo();
        return response;
    }

    // [Route("/all")]
    // [HttpGet]
    // public IEnumerable<HistoricalPrice> GetPrice() {
    //     var BinanceClient = new BinanceClientAdapter();
    //     var response = BinanceClient.GetSpotPrice();
    //     return response;
    // }

}