using Microsoft.AspNetCore.Mvc;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.External.Binance;

namespace Nummi.Api.Controllers; 

[Route("api/market")]
[ApiController]
public class MarketDataController : ControllerBase {

    [Route("")]
    [HttpGet]
    public IEnumerable<HistoricalPrice> GetPrice(IList<string> symbols) {
        var client = new BinanceClient();
        var response = client.GetSpotPrice(symbols);
        return response;
    }

    [Route("/all")]
    [HttpGet]
    public IEnumerable<HistoricalPrice> GetPrice() {
        var client = new BinanceClient();
        var response = client.GetSpotPrice();
        return response;
    }

}