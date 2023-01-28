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

    public StrategyLog Initialize(TradingEnvironment env) {
        var logBuilder = new StrategyLogBuilder(this, env.Mode, StrategyAction.Initializing);
        var context = CreateContext(env, logBuilder);
        
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

    public StrategyLog CheckForTrades(TradingEnvironment env) {
        var logBuilder = new StrategyLogBuilder(this, env.Mode, StrategyAction.Trading);
        var context = CreateContext(env, logBuilder);
        
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

    private StrategyContext CreateContext(TradingEnvironment env, StrategyLogBuilder logBuilder) {
        return new StrategyContext(
            environment: env,
            logBuilder: logBuilder
        );
    }

    protected void Message(string msg) {
        Log.Info($"[{"Type".Purple()}:{GetType().Name.Cyan()}] - {msg}");
    }

    protected abstract void Initialize(StrategyContext context);
    protected abstract void CheckForTrades(StrategyContext context);
    
}