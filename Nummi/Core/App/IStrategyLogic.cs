using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto;

namespace Nummi.Core.App; 

public interface IStrategyLogic {
    public void Initialize(IStrategyContext ctx);
    public void CheckForTrades(IStrategyContext ctx);
}

public interface IStrategyLogicBuiltin : IStrategyLogic {
    public Ksuid Id { get; }
    public string Name { get; }
    public TimeSpan Frequency { get; }
    public Type ParameterType { get; }
    public Type StateType { get; }
}