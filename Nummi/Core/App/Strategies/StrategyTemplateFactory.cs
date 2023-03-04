using Nummi.Core.Config;
using Nummi.Core.Domain.Strategies;
using Nummi.Core.Exceptions;

namespace Nummi.Core.App.Strategies; 

public class StrategyTemplateFactory {

    private static StrategyFrequency MinFrequency => StrategyFrequency.Values[0];

    public StrategyTemplate CreateBuiltinTemplate(IStrategyLogicBuiltin builtinLogic) {
        Type logicType = builtinLogic.GetType();
        Guid id = builtinLogic.Id;
        string? name = (string?) builtinLogic.Name;
        StrategyFrequency frequency = builtinLogic.Frequency;
        Type parameterType = builtinLogic.ParameterType;
        Type stateType = builtinLogic.StateType;
        
        if (name == null) {
            throw new SystemArgumentException($"C# Strategy {builtinLogic.GetType().Name} is missing a Name()");
        }
        if (frequency < MinFrequency) {
            throw new SystemArgumentException(
                $"C# Strategy {builtinLogic.GetType().Name} Frequency() is below the minimum {MinFrequency}"
            );
        }
        if (parameterType == null) {
            throw new SystemArgumentException(
                $"C# Strategy {builtinLogic.GetType().Name} ParameterType cannot be null"
            );
        }
        if (stateType == null) {
            throw new SystemArgumentException(
                $"C# Strategy {builtinLogic.GetType().Name} StateType cannot be null"
            );
        }
        
        StrategyTemplateVersionBuiltin firstTemplateVersion = new(
            version: 0u,
            name: name,
            frequency: frequency,
            logicTypeName: logicType.FullName!,
            parameterTypeName: parameterType.FullName!,
            stateTypeName: stateType.FullName!
        );

        return new StrategyTemplate(
            id: StrategyTemplateId.FromGuid(id), 
            owningUserId: Configuration.ADMIN_USER_ID,
            name: builtinLogic.Name,
            firstVersion: firstTemplateVersion
        );
    }

    // private void ValidateTemplate(StrategyTemplate template) {
    //     var latestVersion = template.Versions.MaxBy(v => v.VersionNumber);
    //     if (latestVersion == null) {
    //         throw new SystemArgumentException($"StrategyTemplate {template.Name} has no versions");
    //     }
    //     
    //     var strategy = latestVersion.Instantiate()
    // }
}