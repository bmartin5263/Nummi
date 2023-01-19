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
    
    // Has this strategy been initialized?
    public bool Initialized { get; private set; }
    
    // Profit generated by this Strategy, may be negative :(
    public decimal Profit { get; private set; }
    
    // How many times this trading strategy checked for trades
    public uint TimesExecuted { get; private set; }
    
    // When was the last time this strategy ran?
    public DateTime? LastExecutedAt { get; private set; }
    
    // How many times this trading strategy threw an exception
    public uint TimesFailed { get; private set; }

    public virtual IList<StrategyLog> Logs { get; private set; } = new List<StrategyLog>();

    protected Strategy(TimeSpan frequency) {
        Frequency = frequency;
    }

    public StrategyLog CheckForTrades(TradingContext context) {
        var logBuilder = new StrategyLogBuilder(this, context.Environment);
        try {
            if (!Initialized) {
                OnInitialize(context);
                Initialized = true;
            }
        
            var startTime = DateTime.Now;
            LastExecutedAt = startTime;
            ++TimesExecuted;
            CheckForTrades(new TradingInterface(logBuilder));
            
            return logBuilder.Build();
        }
        catch (Exception e) {
            Message($"Exception was thrown during CheckForTrades(): {e}");
            ++TimesFailed;
            logBuilder.Error = e;
            throw new StrategyException(logBuilder.Build(), e);
        }
    }

    public void AddProfit(decimal amount) {
        Profit += amount;
    }
    
    protected void Message(string msg) {
        Log.Info($"[{"Type".Purple()}:{GetType().Name.Cyan()}] - {msg}");
    }

    protected abstract void OnInitialize(TradingContext context);
    protected abstract void CheckForTrades(TradingInterface context);
    
}