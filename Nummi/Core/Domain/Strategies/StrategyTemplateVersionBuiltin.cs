namespace Nummi.Core.Domain.Strategies; 

public class StrategyTemplateVersionBuiltin : StrategyTemplateVersion {
    public string LogicTypeName { get; set; }
    public string ParameterTypeName { get; set; }
    public string StateTypeName { get; set; }
    
    protected StrategyTemplateVersionBuiltin() {
        LogicTypeName = null!;
        ParameterTypeName = null!;
        StateTypeName = null!;
    }

    public StrategyTemplateVersionBuiltin(
        uint version,
        string name,
        TimeSpan frequency,
        string logicTypeName, 
        string parameterTypeName, 
        string stateTypeName
    ): base(version, name, frequency, null, false) {
        LogicTypeName = logicTypeName;
        ParameterTypeName = parameterTypeName;
        StateTypeName = stateTypeName;
    }

    protected override Strategy DoInstantiate(string? parameters) {
        return new StrategyBuiltin(this, parameters);
    }
}