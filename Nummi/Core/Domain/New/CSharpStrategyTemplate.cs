using Nummi.Core.Exceptions;

namespace Nummi.Core.Domain.New; 

public class CSharpStrategyTemplate : StrategyTemplate {
    public string StrategyTypeName { get; private set; }
    public string? ParameterTypeName { get; private set; }
    public string? StateTypeName { get; private set; }

    public override bool AcceptsParameters => ParameterTypeName != null;

    protected CSharpStrategyTemplate() {
        StrategyTypeName = null!;
    }

    public CSharpStrategyTemplate(
        string owningUserId,
        string name, 
        TimeSpan frequency,
        string strategyTypeName, 
        ulong version = 0,
        string? parameterTypeName = null, 
        string? stateTypeName = null
    ): base(owningUserId, name, frequency, version) {
        StrategyTypeName = strategyTypeName;
        ParameterTypeName = parameterTypeName;
        StateTypeName = stateTypeName;
    }

    public override Strategy Instantiate(string? parameters) {
        if (AcceptsParameters && parameters == null) {
            throw new InvalidUserArgumentException($"Strategy Template {Name} requires non-null parameters");
        }
        var strategy = new CSharpStrategy(this) {
            ParametersJson = parameters
        };
        strategy.Load();
        return strategy;
    }

    protected override StrategyTemplate CreateNewVersion() {
        return new CSharpStrategyTemplate(UserId, Name, Frequency, StrategyTypeName, Version + 1, ParameterTypeName, StateTypeName);
    }
}