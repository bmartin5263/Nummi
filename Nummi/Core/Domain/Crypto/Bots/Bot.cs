using System.ComponentModel.DataAnnotations.Schema;
using KSUID;
using Microsoft.EntityFrameworkCore;
using NLog;
using Nummi.Core.Domain.Crypto.Strategies;
using Nummi.Core.Domain.Crypto.Strategies.Log;
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

    public StrategyLog? LastStrategyLog { get; private set; }
    
    public bool InErrorState { get; private set; }

    // How much money is available for trading
    public decimal Funds { get; private set; }

    public TradingMode Mode { get; private set; }

    public ISet<Simulation> Simulations { get; } = new HashSet<Simulation>();

    public bool IsSimulationBot => Mode == TradingMode.Simulated;

    public Bot(string name, decimal funds, TradingMode mode) {
        Name = name;
        Funds = funds;
        Mode = mode;
    }
    
    public void AllocateFunds(decimal amount) {
        Funds += amount;
    }

    public void AddSimulation(Simulation simulation) {
        Simulations.Add(simulation);
    }

    public StrategyLog RunRealtime(BotContext botContext) {
        if (Mode == TradingMode.Simulated) {
            throw new InvalidStateException("Cannot run realtime strategies as a simulation bot");
        }

        StrategyLog log;
        try {
            Message($"Running Trading Strategy");
            log = DoRunRealtime(botContext);
            return log;
        }
        catch (StrategyException e) {
            Message($"Strategy threw an exception. LogId: {e.Log.Id.Red()}");
            log = e.Log;
            InErrorState = true;
        }

        SaveLog(log);
        return log;
    }

    public StrategyLog InitializeStrategy(TradingContextFactory contextFactory) {
        var ctx = contextFactory.Create(Mode, Funds, new ClockLive());
        return InitializeStrategy(ctx);
    }

    public StrategyLog InitializeStrategy(ITradingContext ctx) {
        Message($"{"Initializing".Blue()} Trading Strategy");
        var log = Strategy!.Initialize(ctx);
        SaveLog(log);
        return log;
    }

    private StrategyLog DoRunRealtime(BotContext context) {
        var ctx = context.TradingContextFactory.Create(Mode, Funds, new ClockLive());

        StrategyLog log;
        if (Strategy!.ShouldInitialize()) {
            Message($"{"Initializing".Blue()} Trading Strategy");
            log = Strategy.Initialize(ctx);
            SaveLog(log);
        }
        
        log = Strategy.CheckForTrades(ctx);
        return log;
    }

    public List<StrategyLog> RunSimulation(BotContext context, SimulationParameters simulation) {
        if (Strategy == null) {
            throw new InvalidStateException("Cannot run simulation without a Strategy");
        }
        
        // Convert the input to UTC time
        var startTime = simulation.StartTime.ToUniversalTime();
        var endTime = simulation.EndTime.ToUniversalTime();
        var duration = endTime - startTime;

        var logs = new List<StrategyLog>();

        var clock = new ClockMock(startTime);
        var ctx = context.TradingContextFactory.Create(Mode, Funds, clock);
        
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
        var lastExecutionTime = LastStrategyLog?.StartTime ?? DateTime.MinValue;
        if (lastExecutionTime + Strategy!.Frequency > now) {
            sleep = Strategy.Frequency - (now - lastExecutionTime);
            return false;
        }

        sleep = null;
        return true;
    }

    private void SaveLog(StrategyLog log) {
        Strategy!.Logs.Add(log);
        LastStrategyLog = log;
    }
    
    public override string ToString() {
        return this.ToFormattedString();
    }

    private void Message(string msg) {
        Log.Info($"[{"Name".Purple()}:{Name.Cyan()}] - {msg}");
    }
}