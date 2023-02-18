using Nummi.Core.Domain.Common;
using Nummi.Core.Exceptions;

namespace Nummi.Core.Domain.New;

public class Bot : Audited {
    // Unique identifier for this Bot
    public Ksuid Id { get; } = Ksuid.Generate();
    
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
        return CurrentActivation;
    }

    public void Deactivate() {
        if (!IsActive) {
            throw new InvalidUserArgumentException("Cannot deactivate inactive Bot");
        }
        ActivationHistory.Add(CurrentActivation!);
        CurrentActivation = null;
    }

    public void ChangeActiveStrategy(Strategy strategy) {
        if (!IsActive) {
            throw new InvalidUserArgumentException("Cannot change strategy of an inactive Bot");
        }
        Deactivate();
        Activate(strategy);
    }
}