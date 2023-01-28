using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using KSUID;
using Microsoft.EntityFrameworkCore;
using NLog;
using Nummi.Core.Domain.Crypto.Client;
using Nummi.Core.Domain.Crypto.Strategies;
using Nummi.Core.Exceptions;
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

    public Strategy? Strategy { get; set; }

    public StrategyLog? LastStrategyLog { get; private set; }
    
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
        Funds += amount;
    }

    public TimeSpan RunRealtime(BotContext botContext) {
        if (Mode == TradingMode.Simulated) {
            throw new InvalidStateException("Cannot run realtime strategies as a simulation bot");
        }
        
        if (Strategy == null) {
            Message("No strategy assigned");
            return TimeSpan.FromSeconds(5);
        }
        if (InErrorState) {
            Message("Cannot run strategy while in error state");
            return TimeSpan.FromSeconds(5);
        }
        if (IsNotYetTimeToExecute(out TimeSpan? sleep)) {
            Message($"Not yet time to run strategy");
            return sleep!.Value;
        }

        try {
            Message($"Running Trading Strategy");
            DoRunRealtime(botContext);
        }
        catch (StrategyException e) {
            Message($"Strategy threw an exception. LogId: {e.Log.Id.Red()}");
            SaveLog(e.Log);
            InErrorState = true;
        }

        return Strategy.Frequency;
    }

    private void DoRunRealtime(BotContext context) {
        var env = new TradingEnvironment(
            mode: Mode,
            dataClient: context.GetScoped<CryptoDataClientLive>(),
            allowance: Funds,
            appDb: context.AppDb,
            clock: new ClockLive()
        );

        StrategyLog log;
        if (ShouldInitializeStrategy(out TimeSpan? elapsed)) {
            Message($"{"Initializing".Blue()} Trading Strategy after not executing for {elapsed!.Value.ToString().Yellow()}");
            log = Strategy!.Initialize(env);
            SaveLog(log);
        }
        
        log = Strategy!.CheckForTrades(env);
        SaveLog(log);
    }

    public List<StrategyLog> RunSimulation(BotContext context, SimulationParameters simulation) {
        if (Strategy == null) {
            throw new InvalidStateException("Cannot run simulation without a Strategy", HttpStatusCode.BadRequest);
        }
        
        // Force the input to be in local time
        var startTime = simulation.StartTime.ToUniversalTime();
        var endTime = simulation.EndTime.ToUniversalTime();
        var duration = endTime - startTime;

        var logs = new List<StrategyLog>();

        var clock = new ClockMock(startTime);
        var env = new TradingEnvironment(
            mode: TradingMode.Simulated,
            dataClient: context.GetScoped<CryptoDataClientDbProxy>(),
            allowance: Funds,
            appDb: context.AppDb,
            clock: clock
        );
        
        Message($"{"Starting Simulation".Yellow()} (start={simulation.StartTime}, startUTC={startTime}, end={simulation.EndTime}, endUTC={endTime}, duration={duration}, clock.nowUTC={clock.NowUtc}, clock.now={clock.Now})");

        StrategyLog log = Strategy!.Initialize(env);
        logs.Add(log);
        while (clock.NowUtc < endTime) {
            log = Strategy!.CheckForTrades(env);
            logs.Add(log);
            
            clock.ChangeTime(time => time.Add(Strategy!.Frequency));
        }
        
        Message($"{"Finished Simulation".Green()}");

        return logs;
    }

    public void ClearErrorState() {
        InErrorState = false;
    }
    
    private bool IsNotYetTimeToExecute(out TimeSpan? sleep) {
        var now = DateTime.UtcNow;
        var lastExecutionTime = LastStrategyLog?.StartTime ?? DateTime.MinValue;
        if (lastExecutionTime + Strategy!.Frequency > now) {
            sleep = Strategy.Frequency - (now - lastExecutionTime);
            return true;
        }

        sleep = null;
        return false;
    }
    
    private bool ShouldInitializeStrategy(out TimeSpan? elapsed) {
        elapsed = TimeSpan.Zero;
        return true;
        var startTime = DateTime.UtcNow;
        elapsed = startTime - Strategy!.LastExecutedAt;
        return elapsed != null && elapsed >= Strategy.Frequency * 2;
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