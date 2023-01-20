using Microsoft.AspNetCore.Mvc;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.External.Binance;

namespace Nummi.Api.Controllers; 

[Route("api/market")]
[ApiController]
public class MarketDataController : ControllerBase {
    
    private BinanceClient BinanceClient { get; }

    public MarketDataController(BinanceClient binanceClient) {
        BinanceClient = binanceClient;
    }

    [Route("price")]
    [HttpGet]
    public IEnumerable<Price> GetPrice([FromQuery] HashSet<string> symbols) {
        var response = BinanceClient.GetSpotPrice(symbols);
        return response;
    }

    [Route("bars")]
    [HttpGet]
    public IDictionary<string, MinuteBar> GetBars([FromQuery] string symbol) {
        var response = BinanceClient.GetMinuteKlines(new HashSet<string> {symbol});
        return response;
    }
    
    [Route("get-bars")]
    [HttpPost]
    public object GetBars([FromQuery] DateTime? startTime, [FromBody] HashSet<string> symbols) {
        if (startTime == null) {
            return BinanceClient.GetMinuteKlines(symbols);
        }
        DateTime ut = DateTime.SpecifyKind((DateTime)startTime, DateTimeKind.Utc);
        return BinanceClient.GetMinuteKlines(symbols, ut);
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