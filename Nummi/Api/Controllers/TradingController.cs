using Microsoft.AspNetCore.Mvc;
using Nummi.Core.Domain.Crypto.Ordering;
using Nummi.Core.Domain.Crypto.Strategies;

namespace Nummi.Api.Controllers;

[Route("api/stock")]
[ApiController]
public class TradingController : ControllerBase {

    private readonly OrderService orderService;

    public TradingController(
        OrderService orderService
    ) {
        this.orderService = orderService;
    }

    /// <summary>
    /// Place a new trading order
    /// </summary>
    [HttpPost]
    [Route("order")]
    public async Task<Order> PlaceOrder([FromBody] PlaceOrderRq placeOrderRq, [FromQuery] TradingMode mode = TradingMode.Paper) {
        var order = orderService.PlaceOrderAsync(mode, placeOrderRq);
        return await order;
    }

    /// <summary>
    /// Place a new market order
    /// </summary>
    [HttpPost]
    [Route("order/market")]
    public async Task<Order> PlaceOrder([FromBody] MarketOrderRq marketOrderRq, [FromQuery] TradingMode mode = TradingMode.Paper) {
        var order = orderService.PlaceOrderAsync(mode, marketOrderRq.ToPlaceOrderRq());
        return await order;
    }

    /// <summary>
    /// Place a new limit order
    /// </summary>
    [HttpPost]
    [Route("order/limit")]
    public async Task<Order> PlaceOrder([FromBody] LimitOrderRq limitOrderRq, [FromQuery] TradingMode mode = TradingMode.Paper) {
        var order = orderService.PlaceOrderAsync(mode, limitOrderRq.ToPlaceOrderRq());
        return await order;
    }

    [HttpGet]
    [Route("crash")]
    public void Crash() {
        throw new BadHttpRequestException("Bad stuff");
    }
}