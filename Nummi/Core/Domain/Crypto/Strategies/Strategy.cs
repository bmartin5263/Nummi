using System.Text.Json.Serialization;
using KSUID;
using Microsoft.EntityFrameworkCore;
using NLog;
using Nummi.Core.Domain.Crypto.Bots;
using Nummi.Core.Domain.Crypto.Log;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Strategies; 

[PrimaryKey("Id")]
[JsonConverter(typeof(Serializer.AbstractTypeConverter<Strategy>))]
public abstract class Strategy {
    
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    // Unique id for this strategy
    public string Id { get; } = Ksuid.Generate().ToString();

    // How often should this strategy check for possible trades
    public TimeSpan Frequency { get; }

    // When was the last time this strategy ran?
    public DateTime? LastExecutedAt => LastLog?.StartTime;

    public StrategyLog? LastLog { get; set; }
    
    public ISet<Simulation> Simulations { get; } = new HashSet<Simulation>();

    protected Strategy(TimeSpan frequency) {
        Frequency = frequency;
    }

    public StrategyLog Initialize(ITradingContext ctx) {
        var logBuilder = new StrategyLogBuilder(this, ctx.Mode, StrategyAction.Initializing, ctx.BotId);
        var context = new TradingContextAudited(ctx, logBuilder);
        
        try {
            Initialize(context);
        }
        catch (Exception e) {
            Message($"Exception was thrown during Initialize(): {e}");
            logBuilder.Error = e;
            throw new StrategyException(logBuilder.Build(), e);
        }

        return logBuilder.Build();
    }

    public StrategyLog CheckForTrades(ITradingContext ctx) {
        var logBuilder = new StrategyLogBuilder(this, ctx.Mode, StrategyAction.Trading, ctx.BotId);
        var context = new TradingContextAudited(ctx, logBuilder);
        
        try {
            CheckForTrades(context);
        }
        catch (Exception e) {
            Message($"Exception was thrown during CheckForTrades(): {e}");
            logBuilder.Error = e;
            throw new StrategyException(logBuilder.Build(), e);
        }
        
        return logBuilder.Build();
    }

    public bool ShouldInitialize() {
        if (LastExecutedAt == null) {
            return true;
        }
        var elapsedSinceLastExecution = DateTime.UtcNow - LastExecutedAt;
        return elapsedSinceLastExecution > Frequency * 2;
    }
    
    public void AddSimulation(Simulation simulation) {
        Simulations.Add(simulation);
    }

    protected void Message(string msg) {
        Log.Info($"[{"Type".Purple()}:{GetType().Name.Cyan()}] - {msg}");
    }

    protected abstract void Initialize(TradingContextAudited context);
    protected abstract void CheckForTrades(TradingContextAudited context);
    
}