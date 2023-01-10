using Nummi.Core.Domain.Crypto.Bot.Execution;

namespace Nummi.Core.Domain.Crypto.Bot.Strategy;

public class DefaultTradingStrategy : ITradingStrategy {

    private int Data { get; set; } = 100;
    
    public void Execute(BotExecutionContext context) {
        throw new ArgumentException("Haha");
        // var stockClient = context.GetService<CoinbaseClient>()!;
        // var response = await stockClient.GetSpotPriceAsync("BTCUSD");
        // var price = response.Data.Amount;
        // Console.WriteLine($"Current BTC-USD Price is ${price}");
    }
}