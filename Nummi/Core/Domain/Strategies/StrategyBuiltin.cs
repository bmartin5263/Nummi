using JetBrains.Annotations;
using Nummi.Core.Domain.New;

namespace Nummi.Core.Domain.Strategies; 

public class StrategyBuiltin : Strategy {

    public string StrategyTypeName => (ParentTemplate as StrategyTemplateVersionBuiltin)!.LogicTypeName;
    public string? ParameterTypeName => (ParentTemplate as StrategyTemplateVersionBuiltin)!.ParameterTypeName;
    public string? StateTypeName => (ParentTemplate as StrategyTemplateVersionBuiltin)!.StateTypeName;

    [UsedImplicitly]
    protected StrategyBuiltin() {
        
    }

    public StrategyBuiltin(StrategyTemplateVersionBuiltin parentTemplate) : base(parentTemplate) {
        
    }
    
    protected override IStrategyLogic DoCreateLogic() {
        var strategyType = Type.GetType(StrategyTypeName)!;
        return (IStrategyLogic) Activator.CreateInstance(strategyType)!;
    }

    protected override object DoDeserializeParameters(string parametersJson) {
        return ParseJson(parametersJson, ParameterTypeName!);
    }

    protected override object DoDeserializeState(string stateJson) {
        return ParseJson(stateJson, StateTypeName!);
    }
}