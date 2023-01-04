using Alpaca.Markets;
using TestWebApp.Core.Database;
using TestWebApp.Core.Domain.Stocks.Client;

namespace TestWebApp.Core.Domain.Stocks.Ordering;

public class OrderService {

    private readonly AppDb appDb;
    private readonly IStockClient stockClient;

    public OrderService(AppDb appDb, IStockClient stockClient) {
        this.appDb = appDb;
        this.stockClient = stockClient;
    }

    public async Task<Order> PlaceOrderAsync(PlaceOrderRq request) {
        var order = await stockClient.PlaceOrderAsync(request);
        appDb.Orders.Add(order);
        await appDb.SaveChangesAsync();
        return order;
    }

    public Order GetOrder(Guid id) {
        var order = appDb.Orders.FindById(id);
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