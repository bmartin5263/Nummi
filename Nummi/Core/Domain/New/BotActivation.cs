using Nummi.Core.Domain.Common;

namespace Nummi.Core.Domain.New; 

public class BotActivation : Audited {
    // Unique identifier for this Bot
    public Ksuid Id { get; } = Ksuid.Generate();
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
    
    public Strategy Strategy { get; }
    
    public TradingMode Mode { get; }

    public List<BotLog> Logs { get; } = new();

    protected BotActivation() {
        Strategy = null!;
    }

    public BotActivation(Strategy strategy, TradingMode mode) {
        Strategy = strategy;
        Mode = mode;
    }
}