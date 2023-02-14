using System.ComponentModel.DataAnnotations.Schema;
using KSUID;
using Microsoft.EntityFrameworkCore;
using NLog;
using Nummi.Core.Domain.Crypto.Log;
using Nummi.Core.Domain.Crypto.Strategies;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Bots; 

// If in the future I switch to a Strategy approach based on real-time socket data we can rename this LazyBot
// since it requires being "woken up" on a regular schedule
[Table("Bot")]
[PrimaryKey("Id")]
public class Bot {
    
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    // Unique identifier for this Bot
    public string Id { get; } = Ksuid.Generate().ToString();
    
    // Human-readable Name for this Bot
    public string Name { get; set; }

    public Strategy? Strategy { get; set; }
    
    public bool InErrorState { get; private set; }

    // How much money is available for trading
    public decimal Funds { get; private set; }

    public TradingMode Mode { get; private set; }
    
    public bool IsSimulationBot => Mode == TradingMode.Simulated;

    public Bot(string name, decimal funds, TradingMode mode) {
        Name = name;
        Funds = funds;
        Mode = mode;
    }
    
    public void AllocateFunds(decimal amount) {
        if (amount < 0) {
            throw new InvalidUserArgumentException($"Cannot allocate negative funds ({Funds})");
        }
        Funds += amount;
    }

    public void SubtractFunds(decimal amount) {
        if (amount < 0) {
            throw new InvalidUserArgumentException($"Cannot subtract negative funds ({Funds})");
        }

        Funds -= amount;
    }

    public void RunRealtime(NummiContext nummiContext) {
        if (Mode == TradingMode.Simulated) {
            throw new InvalidStateException("Cannot run realtime strategies as a simulation bot");
        }

        StrategyLog log;
        try {
            Message($"Running Trading Strategy");
            DoRunRealtime(nummiContext);
        }
        catch (StrategyException e) {
            Message($"Strategy threw an exception. LogId: {e.Log.Id.Red()}");
            InErrorState = true;
        }
    }

    // public StrategyLog InitializeStrategy(TradingContextFactory contextFactory) {
    //     var ctx = contextFactory.Create(Mode, Funds, new ClockLive());
    //     return InitializeStrategy(ctx);
    // }

    public StrategyLog InitializeStrategy(ITradingContext ctx) {
        Message($"{"Initializing".Blue()} Trading Strategy");
        var log = Strategy!.Initialize(ctx);
        // SaveLog(log);
        return log;
    }

    private void DoRunRealtime(NummiContext context) {
        ITradingContext ctx = context.TradingContextFactory.CreateRealtime(this);

        StrategyLog log;
        if (Strategy!.ShouldInitialize()) {
            Message($"{"Initializing".Blue()} Trading Strategy");
            Strategy.Initialize(ctx);
        }
        
        Strategy.CheckForTrades(ctx);
    }

    public List<StrategyLog> RunSimulation(NummiContext context, SimulationParameters simulation) {
        if (Strategy == null) {
            throw new InvalidStateException("Cannot run simulation without a Strategy");
        }
        
        // Convert the input to UTC time
        var startTime = simulation.StartTime.ToUniversalTime();
        var endTime = simulation.EndTime.ToUniversalTime();
        var duration = endTime - startTime;

        var logs = new List<StrategyLog>();

        var clock = new ClockMock(startTime);
        var ctx = context.TradingContextFactory.CreateSimulated(Funds, clock);
        
        Message($"{"Starting Simulation".Yellow()} (start={simulation.StartTime}, startUTC={startTime}, end={simulation.EndTime}, endUTC={endTime}, duration={duration}, clock.nowUTC={clock.NowUtc}, clock.now={clock.Now})");

        StrategyLog log = Strategy!.Initialize(ctx);
        logs.Add(log);
        while (clock.NowUtc < endTime) {
            log = Strategy!.CheckForTrades(ctx);
            logs.Add(log);
            
            clock.ChangeTime(time => time.Add(Strategy!.Frequency));
        }
        
        Message($"{"Finished Simulation".Green()}");

        return logs;
    }

    public void ClearErrorState() {
        InErrorState = false;
    }
    
    public bool IsTimeToExecute(out TimeSpan? sleep) {
        var now = DateTime.UtcNow;
        var lastExecutionTime = Strategy!.LastLog?.StartTime ?? DateTime.MinValue;
        if (lastExecutionTime + Strategy!.Frequency > now) {
            sleep = Strategy.Frequency - (now - lastExecutionTime);
            return false;
        }

        sleep = null;
        return true;
    }

    // private void SaveLog(StrategyLog log) {
    //     Strategy!.Logs.Add(log);
    //     LastStrategyLog = log;
    // }
    
    public override string ToString() {
        return this.ToFormattedString();
    }

    private void Message(string msg) {
        Log.Info($"[{"Name".Purple()}:{Name.Cyan()}] - {msg}");
    }
}