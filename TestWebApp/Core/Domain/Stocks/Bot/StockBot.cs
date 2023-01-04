using System.ComponentModel.DataAnnotations;
using Humanizer;
using TestWebApp.Core.Domain.Stocks.Bot.Strategy;

namespace TestWebApp.Core.Domain.Stocks.Bot; 

public class StockBot {

    private static readonly Random RANDOM = new Random();
    private const string NAME_TEMPLATE = "TRADING_BOT_#{0}";
    
    public Guid Id { get; }
    public string Name { get; set; }
    public ITradingStrategy Strategy { get; set; } = new TradingStrategy();

    public StockBot(): this(GenerateDefaultName()) {
    }

    public StockBot(string name) {
        Id = Guid.NewGuid();
        Name = name;
    }

    private static string GenerateDefaultName() {
        return NAME_TEMPLATE.FormatWith(RANDOM.Next(1000, 9999));
    }
}