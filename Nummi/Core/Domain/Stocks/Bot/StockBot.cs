using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Humanizer;
using KSUID;
using Nummi.Core.Domain.Stocks.Bot.Strategy;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Stocks.Bot; 

public class StockBot {

    private static readonly Random RANDOM = new();
    private const string NAME_TEMPLATE = "TRADING_BOT_#{0}";
    
    public Ksuid Id { get; }
    public string Name { get; set; }
    public ITradingStrategy? Strategy { get; set; }
    public decimal AvailableCash { get; private set; }
    public decimal Profit { get; private set; }

    public StockBot(): this(GenerateDefaultName()) { }

    public StockBot(string name) {
        Id = Ksuid.Generate();
        Name = name;
    }

    private static string GenerateDefaultName() {
        return NAME_TEMPLATE.FormatWith(RANDOM.Next(1000, 9999));
    }

    public void AddAvailableCash(decimal amount) {
        AvailableCash += amount;
    }
    
    public void AddProfit(decimal amount) {
        Profit += amount;
    }
    
    public override string ToString() {
        return this.ToFormattedString();
    }
}