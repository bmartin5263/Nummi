using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using KSUID;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Domain.Crypto.Client;
using Nummi.Core.Domain.Crypto.Strategies;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Bots; 

[Table("Bot")]
[PrimaryKey("Id")]
[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
public class Bot {

    // Unique identifier for this Bot
    public string Id { get; } = Ksuid.Generate().ToString();
    
    // Human-readable Name for this Bot
    public string Name { get; set; }

    [ForeignKey("StrategyId")]
    public virtual Strategy? Strategy { get; set; }

    // How much money is available for trading
    public decimal Funds { get; private set; }

    public Bot(string name, decimal funds) {
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
        var binanceClient = env.GetService<BinanceClientAdapter>();
        var tradingContext = new TradingContext(cryptoClient, Funds, binanceClient, env.AppDb);
        var result = Strategy!.CheckForTrades(tradingContext);
    }

    public override string ToString() {
        return this.ToFormattedString();
    }

    private void Message(string msg) {
        Console.WriteLine($"{Name} - {msg}");
    }
}