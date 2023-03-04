using System.ComponentModel.DataAnnotations.Schema;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.Domain.Strategies;
using Nummi.Core.Events;
using Nummi.Core.Exceptions;

namespace Nummi.Core.Domain.Bots;

public readonly record struct BotId(Guid Value) {
    public override string ToString() => Value.ToString("N");
    public static BotId Generate() => new(Guid.NewGuid());
    public static BotId FromGuid(Guid id) => new(id);
    public static BotId FromString(string s) => new(Guid.ParseExact(s, "N"));
}

public class Bot : Audited, EventPublisher {
    [NotMapped]
    public IList<IDomainEvent> DomainEvents { get; } = new List<IDomainEvent>();

    public BotId Id { get; } = BotId.Generate();
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
    
    // Human-readable Name for this Bot
    public string Name { get; set; }
    
    // How much money is available for trading
    public decimal Funds { get; private set; }
    
    public TradingMode Mode { get; private set; }
    
    public bool InErrorState { get; private set; }

    public BotActivation? CurrentActivation { get; private set; }

    public bool IsActive => CurrentActivation != null;

    public List<BotActivation> ActivationHistory { get; } = new();
    
    public Bot(string name, TradingMode mode, decimal funds) {
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

    public BotActivation Activate(Strategy strategy) {
        if (IsActive) {
            throw new InvalidUserArgumentException("Cannot activate an already active Bot");
        }
        CurrentActivation = new BotActivation(strategy, Mode);
        ActivationHistory.Add(CurrentActivation);
        DomainEvents.Add(new BotActivatedEvent(Id));
        return CurrentActivation;
    }
    
    public BotActivation Reactivate() {
        if (IsActive) {
            throw new InvalidUserArgumentException("Cannot reactivate an already active Bot");
        }

        var lastActivation = ActivationHistory[0];
        CurrentActivation = lastActivation;
        InErrorState = false;
        DomainEvents.Add(new BotActivatedEvent(Id));
        
        return CurrentActivation;
    }

    public void Deactivate() {
        if (!IsActive) {
            throw new InvalidUserArgumentException("Cannot deactivate inactive Bot");
        }
        DomainEvents.Add(new BotDeactivatedEvent(Id));
        CurrentActivation = null;
    }

    public void ChangeActiveStrategy(Strategies.Strategy strategy) {
        if (!IsActive) {
            throw new InvalidUserArgumentException("Cannot change strategy of an inactive Bot");
        }
        Deactivate();
        Activate(strategy);
    }

    public void SetInErrorState() {
        InErrorState = true;
    }
}