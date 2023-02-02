using System.Text.Json.Serialization;
using KSUID;
using Microsoft.EntityFrameworkCore;
using NLog;
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
    
    // How many times this trading strategy checked for trades
    public int TimesExecuted => Logs.Count;
    
    // When was the last time this strategy ran?
    public DateTime? LastExecutedAt => Logs.MaxOrDefault(l => l.StartTime);
    
    // How many times this trading strategy threw an exception
    public int TimesFailed => Logs.Count(l => l.Error != null);

    public virtual IList<StrategyLog> Logs { get; private set; } = new List<StrategyLog>();

    protected Strategy(TimeSpan frequency) {
        Frequency = frequency;
    }

    public StrategyLog Initialize(TradingContext ctx) {
        var logBuilder = new StrategyLogBuilder(this, ctx.Mode, StrategyAction.Initializing);
        var context = new StrategyContext(ctx, logBuilder);
        
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

    public StrategyLog CheckForTrades(TradingContext ctx) {
        var logBuilder = new StrategyLogBuilder(this, ctx.Mode, StrategyAction.Trading);
        var context = new StrategyContext(ctx, logBuilder);
        
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

    protected void Message(string msg) {
        Log.Info($"[{"Type".Purple()}:{GetType().Name.Cyan()}] - {msg}");
    }

    protected abstract void Initialize(StrategyContext context);
    protected abstract void CheckForTrades(StrategyContext context);
    
}