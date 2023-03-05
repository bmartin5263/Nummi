using Nummi.Core.Domain.Crypto;
using Nummi.Core.Domain.Strategies;

namespace Nummi.Core.App.Strategies; 

public interface IStrategyLogic {
    public bool NeedsInitialization(TimeSpan sinceLastExecution);
    public void Initialize(IStrategyContext ctx);
    public void CheckForTrades(IStrategyContext ctx);
}

public interface IStrategyLogicBuiltin : IStrategyLogic {
    public Guid Id { get; }
    public string Name { get; }
    public StrategyFrequency Frequency { get; }
    public Type ParameterType { get; }
    public Type StateType { get; }

    bool IStrategyLogic.NeedsInitialization(TimeSpan sinceLastExecution) {
        return sinceLastExecution > Frequency.AsTimeSpan * 2;
    }
}