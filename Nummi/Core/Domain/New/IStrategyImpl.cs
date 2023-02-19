namespace Nummi.Core.Domain.New; 

public interface IStrategyImpl {
    public void Initialize(ITradingContext ctx, object? parameters, ref object? state);
    public void CheckForTrades(ITradingContext ctx, object? parameters, ref object? state);
}

public interface ICSharpStrategyImpl : IStrategyImpl {
}

public interface ICSharpStrategyImpl<in P, S> : ICSharpStrategyImpl where P : class where S : class {
    
    void IStrategyImpl.Initialize(ITradingContext ctx, object? parameters, ref object? state) {
        S? casted = state as S;
        Initialize(ctx, parameters as P, ref casted);
        state = casted;
    }

    public void Initialize(ITradingContext ctx, P? parameters, ref S? state);
    
    void IStrategyImpl.CheckForTrades(ITradingContext ctx, object? parameters, ref object? state) {
        S? casted = state as S;
        CheckForTrades(ctx, parameters as P, ref casted);
        state = casted;
    }

    public void CheckForTrades(ITradingContext ctx, P? parameters, ref S? state);
}