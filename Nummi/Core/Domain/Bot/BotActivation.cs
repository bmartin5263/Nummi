using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto;

namespace Nummi.Core.Domain.New; 

public class BotActivation : Audited {
    // Unique identifier for this Bot
    public Ksuid Id { get; } = Ksuid.Generate();
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
    
    public Strategies.Strategy Strategy { get; }
    
    public TradingMode Mode { get; }

    public List<BotLog> Logs { get; } = new();

    protected BotActivation() {
        Strategy = null!;
    }

    public BotActivation(Strategies.Strategy strategy, TradingMode mode) {
        Strategy = strategy;
        Mode = mode;
    }
}