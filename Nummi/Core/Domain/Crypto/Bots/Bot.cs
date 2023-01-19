using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using KSUID;
using Microsoft.EntityFrameworkCore;
using NLog;
using Nummi.Core.Domain.Crypto.Client;
using Nummi.Core.Domain.Crypto.Strategies;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Bots; 

// If in the future I switch to a Strategy approach based on real-time socket data we can rename this LazyBot
// since it requires being "woken up" on a regular schedule
[Table("Bot")]
[PrimaryKey("Id")]
[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
public class Bot {
    
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    // Unique identifier for this Bot
    public string Id { get; } = Ksuid.Generate().ToString();
    
    // Human-readable Name for this Bot
    public string Name { get; set; }

    public virtual Strategy? Strategy { get; set; }

    public virtual StrategyLog? LastStrategyLog { get; private set; }
    
    public bool InErrorState { get; private set; }

    // How much money is available for trading
    public decimal Funds { get; private set; }

    public TradingEnvironment Environment;

    public Bot(string name, decimal funds) {
        Name = name;
        Funds = funds;
    }
    
    public void AllocateFunds(decimal amount) {
        Funds += amount;
    }

    public TimeSpan WakeUp(ApplicationContext env) {
        if (Strategy == null) {
            Message("No strategy assigned");
            return TimeSpan.FromSeconds(5);
        }
        
        if (InErrorState) {
            Message("Cannot run strategy while in error state");
            return TimeSpan.FromSeconds(5);
        }

        var now = DateTime.Now;
        var lastExecutionTime = LastStrategyLog?.StartTime ?? DateTime.MinValue;
        if (lastExecutionTime + Strategy.Frequency > now) {
            var sleepTime = Strategy.Frequency - (now - lastExecutionTime);
            Message("Not yet time to run strategy. Next execution in " + sleepTime);
            return sleepTime;
        }

        try {
            Message($"Running Trading Strategy");
            RunTradingStrategy(env);
        }
        catch (StrategyException e) {
            Message($"Strategy threw an exception. {e.Log}");
            LastStrategyLog = e.Log;
            InErrorState = true;
        }

        return Strategy.Frequency;
    }

    private void RunTradingStrategy(ApplicationContext env) {
        var cryptoClient = env.GetService<CryptoClientMock>();
        var binanceClient = env.GetService<BinanceClientAdapter>();
        var tradingContext = new TradingContext(Environment, cryptoClient, Funds, binanceClient, env.AppDb);
        LastStrategyLog = Strategy!.CheckForTrades(tradingContext);
    }

    public override string ToString() {
        return this.ToFormattedString();
    }

    private void Message(string msg) {
        Log.Info($"[{"Name".Purple()}:{Name.Cyan()}] - {msg}");
    }

    public void ClearErrorState() {
        InErrorState = false;
    }
}