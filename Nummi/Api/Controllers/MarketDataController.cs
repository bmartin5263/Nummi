using Microsoft.AspNetCore.Mvc;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.External.Alpaca;
using Nummi.Core.External.Binance;

namespace Nummi.Api.Controllers; 

[Route("api/market")]
[ApiController]
public class MarketDataController : ControllerBase {
    
    private BinanceClientAdapter BinanceClient { get; }
    private IAlpacaClient AlpacaClient { get; }

    public MarketDataController(BinanceClientAdapter binanceClient, IAlpacaClient alpacaClient) {
        BinanceClient = binanceClient;
        AlpacaClient = alpacaClient;
    }

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

}