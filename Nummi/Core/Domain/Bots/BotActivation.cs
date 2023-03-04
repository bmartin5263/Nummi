using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.Domain.Strategies;

namespace Nummi.Core.Domain.Bots; 

public readonly record struct BotActivationId(Guid Value) {
    public override string ToString() => Value.ToString("N");
    public static BotActivationId Generate() => new(Guid.NewGuid());
    public static BotActivationId FromGuid(Guid id) => new(id);
    public static BotActivationId FromString(string s) => new(Guid.ParseExact(s, "N"));
}

public class BotActivation : Audited {
    // Unique identifier for this Bot
    public BotActivationId Id { get; } = BotActivationId.Generate();
    
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