using JetBrains.Annotations;
using Nummi.Core.App;

namespace Nummi.Core.Domain.Strategies; 

public class StrategyBuiltin : Strategy {

    public string StrategyTypeName => (ParentTemplateVersion as StrategyTemplateVersionBuiltin)!.LogicTypeName;
    public string ParameterTypeName => (ParentTemplateVersion as StrategyTemplateVersionBuiltin)!.ParameterTypeName;
    public string StateTypeName => (ParentTemplateVersion as StrategyTemplateVersionBuiltin)!.StateTypeName;

    [UsedImplicitly]
    protected StrategyBuiltin() {
        
    }

    public StrategyBuiltin(
        StrategyTemplateVersionBuiltin templateVersion,
        string? parametersJson
    ) : base(templateVersion, parametersJson) {
        
    }
    
    protected override IStrategyLogic DoCreateLogic() {
        var strategyType = Type.GetType(StrategyTypeName)!;
        return (IStrategyLogic) Activator.CreateInstance(strategyType)!;
    }

    protected override object DoDeserializeParameters(string parametersJson) {
        return ParseJson(parametersJson, ParameterTypeName);
    }

    protected override object DoDeserializeState(string stateJson) {
        return ParseJson(stateJson, StateTypeName);
    }

    protected override object CreateDefaultState() {
        var stateType = Type.GetType(StateTypeName)!;
        return Activator.CreateInstance(stateType)!;
    }
}