using Nummi.Core.Domain.New;

namespace Nummi.Core.Domain.Strategies; 

public interface IStrategyLogic {
    public void Initialize(ITradingContext ctx);
    public void CheckForTrades(ITradingContext ctx);
}

public interface IStrategyLogic<P, S> : IStrategyLogic where P : class where S : class {
    void IStrategyLogic.Initialize(ITradingContext ctx) {
        Initialize((ITradingContext<P, S>) ctx);
    }
    void IStrategyLogic.CheckForTrades(ITradingContext ctx) {
        CheckForTrades((ITradingContext<P, S>) ctx);
    }
    
    public void Initialize(ITradingContext<P, S> ctx);
    public void CheckForTrades(ITradingContext<P, S> ctx);
}

public interface IStrategyLogicBuiltin : IStrategyLogic {
    public string Name { get; }
    public TimeSpan Frequency { get; }
}

public interface IStrategyLogicBuiltin<P, S> : IStrategyLogicBuiltin where P : class where S : class {
    void IStrategyLogic.Initialize(ITradingContext ctx) {
        Initialize((ITradingContext<P, S>) ctx);
    }

    void IStrategyLogic.CheckForTrades(ITradingContext ctx) {
        CheckForTrades((ITradingContext<P, S>) ctx);
    }

    public void Initialize(ITradingContext<P, S> ctx);
    public void CheckForTrades(ITradingContext<P, S> ctx);
}