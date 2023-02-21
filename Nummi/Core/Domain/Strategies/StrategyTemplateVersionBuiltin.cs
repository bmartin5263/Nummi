using Nummi.Core.Exceptions;

namespace Nummi.Core.Domain.Strategies; 

public class StrategyTemplateVersionBuiltin : StrategyTemplateVersion {
    public string LogicTypeName { get; set; }
    public string? ParameterTypeName { get; set; }
    public string? StateTypeName { get; set; }

    public override bool AcceptsParameters => ParameterTypeName != null;

    protected StrategyTemplateVersionBuiltin() {
        LogicTypeName = null!;
    }

    public StrategyTemplateVersionBuiltin(
        uint version,
        string name,
        TimeSpan frequency,
        string logicTypeName, 
        string? parameterTypeName = null, 
        string? stateTypeName = null
    ): base(version, name, frequency, null, false) {
        LogicTypeName = logicTypeName;
        ParameterTypeName = parameterTypeName;
        StateTypeName = stateTypeName;
    }

    public override Strategy Instantiate(string? parameters) {
        if (AcceptsParameters && parameters == null) {
            throw new InvalidUserArgumentException($"Strategy Template requires non-null parameters");
        }
        var strategy = new StrategyBuiltin(this) {
            ParametersJson = parameters
        };
        strategy.Load();
        return strategy;
    }
}