using System.Text.Json.Nodes;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Trading.Strategy; 

public static class StrategyFactory {
    
    private static readonly Type[] KNOWN_SUBTYPES = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(domainAssembly => domainAssembly.GetExportedTypes())
        .Where(type => typeof(TradingStrategy).IsAssignableFrom(type))
        .ToArray();

    public static TradingStrategy Create(string name, JsonNode? parameterObject) {
        Console.WriteLine($"Attempting to Instantiate Strategy with name matching '{name}'");
        var lowercase = name.ToLower();
        foreach (var type in KNOWN_SUBTYPES) {
            if (!type.FullName!.ToLower().Contains(lowercase)) {
                continue;
            }
            Console.WriteLine($"Found Strategy with type [{type}]");
            return InstantiateStrategy(type, parameterObject);
        }
        
        throw new ArgumentException($"No Strategy matching name '{name}'");
    }

    public static T Create<T>(string name, JsonNode? parameterObject = null) where T : TradingStrategy {
        return (T) Create(name, parameterObject);
    }

    private static TradingStrategy InstantiateStrategy(Type type, JsonNode? parameterObject) {
        var strategy = (TradingStrategy) Activator.CreateInstance(type)!;
        var strategyImplType = strategy.GetType();

        bool isParameterized = strategyImplType.GetInterfaces()
            .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IParameterizedStrategy<>));
        if (isParameterized) {
            InjectParameterObject(strategy, parameterObject!);
        }

        return strategy;
    }

    private static void InjectParameterObject(TradingStrategy strategy, JsonNode parameterObject) {
        IParameterizedStrategy parameterizedStrategy = (IParameterizedStrategy) strategy;
        var type = parameterizedStrategy.ParameterObjectType;
        var parametersInstance = Serializer.FromJson<object>(parameterObject, type)!;
        parameterizedStrategy.AcceptParameters(parametersInstance);
    }
}