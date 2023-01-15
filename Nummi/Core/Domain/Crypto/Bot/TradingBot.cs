using System.ComponentModel.DataAnnotations.Schema;
using KSUID;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Domain.Crypto.Client;
using Nummi.Core.Domain.Crypto.Trading.Strategy;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Bot; 

[Table("Bot")]
[PrimaryKey("Id")]
public class TradingBot {

    // Unique identifier for this Bot
    public string Id { get; } = Ksuid.Generate().ToString();
    
    // Human-readable Name for this Bot
    public string Name { get; set; }

    [ForeignKey("StrategyId")]
    public virtual TradingStrategy? Strategy { get; set; }

    // How much money is available for trading
    public decimal Funds { get; private set; }

    public TradingBot(string name, decimal funds) {
        Name = name;
        Funds = funds;
    }
    
    public void AllocateFunds(decimal amount) {
        Funds += amount;
    }

    public TimeSpan WakeUp(BotEnvironment env) {
        if (Strategy == null) {
            Message("No Strategy Assigned, going to sleep");
            return TimeSpan.FromSeconds(5);
        }

        try {
            RunTradingStrategy(env);
        }
        catch (StrategyException e) {
            Message($"Strategy threw an exception {e}");
        }

        return Strategy.Frequency;
    }

    private void RunTradingStrategy(BotEnvironment env) {
        var cryptoClient = env.GetService<CryptoClientMock>();
        var tradingContext = new TradingContext(cryptoClient, Funds);
        var result = Strategy!.CheckForTrades(tradingContext);
    }

    public override string ToString() {
        return this.ToFormattedString();
    }

    private void Message(string msg) {
        Console.WriteLine($"{Name} - {msg}");
    }
}