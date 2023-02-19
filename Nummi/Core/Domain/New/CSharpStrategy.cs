using JetBrains.Annotations;

namespace Nummi.Core.Domain.New; 

public class CSharpStrategy : Strategy {

    public string StrategyTypeName => (ParentTemplate as CSharpStrategyTemplate)!.StrategyTypeName;
    public string? ParameterTypeName => (ParentTemplate as CSharpStrategyTemplate)!.ParameterTypeName;
    public string? StateTypeName => (ParentTemplate as CSharpStrategyTemplate)!.StateTypeName;

    [UsedImplicitly]
    protected CSharpStrategy() {
        
    }

    public CSharpStrategy(CSharpStrategyTemplate parentTemplate) : base(parentTemplate) {
        
    }
    
    protected override IStrategyImpl CreateImpl() {
        var strategyType = Type.GetType(StrategyTypeName)!;
        return (IStrategyImpl) Activator.CreateInstance(strategyType)!;
    }

    protected override object DoDeserializeParameters(string parametersJson) {
        return ParseJson(parametersJson, ParameterTypeName!);
    }

    protected override object DoDeserializeState(string stateJson) {
        return ParseJson(stateJson, StateTypeName!);
    }
}