using Coinbase.Models;
using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Domain.Crypto.Ordering;
using Nummi.Core.External.Coinbase;

namespace Nummi.Api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase {

    private readonly ILogger<StockController> logger;
    private readonly OrderService orderService;
    private readonly MarketDataService marketDataService;
    private readonly CoinbaseClient coinbaseClient;

    public StockController(
        ILogger<StockController> logger,
        OrderService orderService,
        MarketDataService marketDataService, 
        CoinbaseClient coinbaseClient
    ) {
        this.logger = logger;
        this.orderService = orderService;
        this.marketDataService = marketDataService;
        this.coinbaseClient = coinbaseClient;
    }

    [HttpGet]
    [Route("{symbol}")]
    public async Task<SnapshotDto> GetSnapshot(string symbol) {
        var snapshot = await marketDataService.GetSnapshot(symbol);
        return snapshot.ToDto();
    }

    [HttpGet]
    [Route("crypto/{symbol}")]
    public async Task<Coinbase.Models.Response<Money>> GetCryptoSnapshot(string symbol) {
        return await coinbaseClient.GetSpotPriceAsync(symbol);
    }

    [HttpPost]
    [Route("order")]
    public async Task<Order> PlaceOrder(PlaceOrderRq placeOrderRq) {
        var order = orderService.PlaceOrderAsync(placeOrderRq);
        return await order;
    }

    [HttpPost]
    [Route("order/market")]
    public async Task<Order> PlaceOrder(MarketOrderRq marketOrderRq) {
        var order = orderService.PlaceOrderAsync(marketOrderRq.ToPlaceOrderRq());
        return await order;
    }

    [HttpPost]
    [Route("order/limit")]
    public async Task<Order> PlaceOrder(LimitOrderRq limitOrderRq) {
        var order = orderService.PlaceOrderAsync(limitOrderRq.ToPlaceOrderRq());
        return await order;
    }

    [HttpGet]
    [Route("crash")]
    public void Crash() {
        throw new BadHttpRequestException("Bad stuff");
    }

    //
    // [HttpPost]
    // [Route("{symbol}/order/{id}/process")]
    // public Order ProcessOrder(string symbol, string id) {
    //     var guid = Guid.Parse(id);
    //     orderService.ProcessOrder(guid);
    //     var order = orderService.GetOrder(guid);
    //     return order;
    // }
}