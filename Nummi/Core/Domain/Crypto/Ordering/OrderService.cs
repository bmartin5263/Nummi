using Alpaca.Markets;
using Nummi.Core.Database;
using Nummi.Core.Domain.Crypto.Client;

namespace Nummi.Core.Domain.Crypto.Ordering;

public class OrderService {

    private readonly AppDb appDb;
    private readonly IStockClient stockClient;

    public OrderService(AppDb appDb, IStockClient stockClient) {
        this.appDb = appDb;
        this.stockClient = stockClient;
    }

    public async Task<Order> PlaceOrderAsync(PlaceOrderRq request) {
        var order = await stockClient.PlaceOrderAsync(request);
        return order;
    }

    private static Order CreateTestOrder(string symbol) {
        return new Order(
            Guid.NewGuid(),
            "ClientOrderId",
            DateTime.Now,
            DateTime.Now,
            DateTime.Now,
            DateTime.Now,
            DateTime.Now,
            DateTime.Now,
            DateTime.Now,
            DateTime.Now,
            Guid.NewGuid(),
            symbol,
            AssetClass.UsEquity,
            1.0m,
            1.2m,
            1.3m,
            10,
            5,
            OrderType.Market,
            OrderClass.Simple,
            OrderSide.Buy,
            TimeInForce.Day,
            null,
            null,
            null,
            null,
            null,
            null,
            OrderStatus.Filled,
            null,
            null
        );
    }
    
}