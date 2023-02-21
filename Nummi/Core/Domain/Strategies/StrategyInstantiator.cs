using Nummi.Core.Config;
using Nummi.Core.Domain.New;
using Nummi.Core.Exceptions;

namespace Nummi.Core.Domain.Strategies; 

public class StrategyInstantiator {
    
    private static TimeSpan MinFrequency => TimeSpan.FromMinutes(1);

    public StrategyTemplate CreateBuiltinTemplate(IStrategyLogicBuiltin builtinLogic) {
        Type logicType = builtinLogic.GetType();
        string? name = (string?) builtinLogic.Name;
        TimeSpan frequency = builtinLogic.Frequency;
        
        if (name == null) {
            throw new SystemArgumentException($"C# Strategy {builtinLogic.GetType().Name} is missing a Name()");
        }
        if (frequency < MinFrequency) {
            throw new SystemArgumentException(
                $"C# Strategy {builtinLogic.GetType().Name} Frequency() is below the minimum {MinFrequency}"
            );
        }
        
        var genericInterface = logicType
            .GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStrategyLogicBuiltin<,>));

        Type? parameterType = null;
        Type? stateType = null;
        if (genericInterface != null) {
            parameterType = genericInterface.GetGenericArguments()[0];
            stateType = genericInterface.GetGenericArguments()[1];
        }

        var firstTemplateVersion = new StrategyTemplateVersionBuiltin(
            version: 0u,
            name: name,
            frequency: frequency,
            logicTypeName: logicType.FullName!,
            parameterTypeName: parameterType?.FullName,
            stateTypeName: stateType?.FullName
        );

        return new StrategyTemplate(
            owningUserId: Configuration.ADMIN_USER_ID,
            name: builtinLogic.Name,
            firstVersion: firstTemplateVersion
        );
    }
    
}