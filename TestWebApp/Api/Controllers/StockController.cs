using Coinbase.Models;
using Microsoft.AspNetCore.Mvc;
using TestWebApp.Api.Model;
using TestWebApp.Core.Database;
using TestWebApp.Core.Domain.Stocks.Bot.Execution;
using TestWebApp.Core.Domain.Stocks.Data;
using TestWebApp.Core.Domain.Stocks.Ordering;
using TestWebApp.Core.External.Alpaca;
using TestWebApp.Core.External.Coinbase;
using YahooFinanceClient.Models;

namespace TestWebApp.Api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase {

    private readonly ILogger<StockController> logger;
    private readonly IAlpacaClient alpacaClient;
    private readonly OrderService orderService;
    private readonly MarketDataService marketDataService;
    private readonly AppDb appDb;
    private readonly BotExecutor botExecutor;
    private readonly CoinbaseClient coinbaseClient;

    public StockController(
        ILogger<StockController> logger, 
        IAlpacaClient alpacaClient,
        OrderService orderService,
        MarketDataService marketDataService, 
        AppDb appDb,
        BotExecutor botExecutor, 
        CoinbaseClient coinbaseClient
    ) {
        this.logger = logger;
        this.alpacaClient = alpacaClient;
        this.orderService = orderService;
        this.marketDataService = marketDataService;
        this.appDb = appDb;
        this.botExecutor = botExecutor;
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
    public async Task<Response<Money>> GetCryptoSnapshot(string symbol) {
        return await coinbaseClient.GetSpotPriceAsync("ETH-USD");
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
    [Route("{symbol}/order/{id}")]
    public Order GetOrderById(string symbol, string id) {
        var guid = Guid.Parse(id);
        var order = orderService.GetOrder(guid);
        return order;
    }

    [HttpGet]
    [Route("crash")]
    public void Crash() {
        throw new BadHttpRequestException("Bad stuff");
    }

    [HttpGet]
    [Route("executor")]
    public uint Executor() {
        return botExecutor.Threads;
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