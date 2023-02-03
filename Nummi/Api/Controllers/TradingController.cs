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
    public Order PlaceOrder([FromBody] OrderRequest orderRequest, [FromQuery] TradingMode mode = TradingMode.Paper) {
        var order = orderService.PlaceOrder(mode, orderRequest);
        return order;
    }

    /// <summary>
    /// Place a new market order
    /// </summary>
    [HttpPost]
    [Route("order/market")]
    public Order PlaceOrder([FromBody] MarketOrderRequest marketOrderRequest, [FromQuery] TradingMode mode = TradingMode.Paper) {
        var order = orderService.PlaceOrder(mode, marketOrderRequest.ToOrderRequest());
        return order;
    }

    /// <summary>
    /// Place a new limit order
    /// </summary>
    [HttpPost]
    [Route("order/limit")]
    public Order PlaceOrder([FromBody] LimitOrderRq limitOrderRq, [FromQuery] TradingMode mode = TradingMode.Paper) {
        var order = orderService.PlaceOrder(mode, limitOrderRq.ToOrderRequest());
        return order;
    }

    [HttpGet]
    [Route("crash")]
    public void Crash() {
        throw new BadHttpRequestException("Bad stuff");
    }
}