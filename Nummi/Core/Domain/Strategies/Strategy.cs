using System.Text.Json.Serialization;
using Nummi.Core.App;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Strategies;

[JsonConverter(typeof(Serializer.AbstractTypeConverter<Strategy>))]
public abstract class Strategy : Audited {
    protected static readonly object EMPTY_OBJECT = new();
    
    public Ksuid Id { get; } = Ksuid.Generate();

    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }

    public StrategyTemplateVersion ParentTemplateVersion { get; }

    public TimeSpan Frequency { get; }
    
    public string? ParametersJson { get; private set; }

    public string? StateJson { get; private set; }
    
    public List<StrategyLog> Logs { get; } = new();

    private bool loaded = false;
    private object? stateInstance;
    private object? parametersInstance;
    private IStrategyLogic? strategyLogic;

    protected Strategy() {
        ParentTemplateVersion = null!;
    }

    protected Strategy(
        StrategyTemplateVersion templateVersion,
        string? parametersJson
    ) {
        ParentTemplateVersion = templateVersion;
        Frequency = templateVersion.Frequency;
        ParametersJson = parametersJson;
        StateJson = null;
    }

    public void ChangeParameters(string? newParametersJson) {
        ParametersJson = newParametersJson;
        parametersInstance ??= DeserializeParameters(ParametersJson);
    }

    private StrategyLogBuilder CreateLogBuilder(ITradingSession session, StrategyAction action) {
        return new StrategyLogBuilder(
            strategy: this,
            mode: session.Mode,
            action: action
        );
    }
    
    public StrategyLog Initialize(ITradingSession session) {
        return WithContext(session, StrategyAction.Initializing, context => strategyLogic!.Initialize(context));
    }
    
    public StrategyLog CheckForTrades(ITradingSession session) {
        return WithContext(session, StrategyAction.Trading, context => strategyLogic!.CheckForTrades(context));
    }

    private StrategyLog WithContext(ITradingSession session, StrategyAction actionName, Action<StrategyContext> action) {
        Load();
        StrategyLogBuilder logBuilder = CreateLogBuilder(session, actionName);
        StrategyContext context = new(session, logBuilder, parametersInstance!, stateInstance!);

        try {
            action(context);
        }
        catch (Exception e) {
            logBuilder.Error = e;
        }

        StrategyLog log = logBuilder.Build();
        Logs.Add(log);

        if (log.Error != null) {
            throw new StrategyException(log, logBuilder.Error);
        }
        return log;
    }

    public void Load() {
        if (loaded) {
            return;
        }
        
        parametersInstance ??= DeserializeParameters(ParametersJson);
        stateInstance ??= DeserializeState(StateJson);
        strategyLogic ??= DoCreateLogic();
        
        loaded = true;
    }

    public void Save() {
        if (!loaded) {
            return;
        }
        
        ParametersJson = !ReferenceEquals(parametersInstance, EMPTY_OBJECT) ? Serializer.ToJson(parametersInstance) : null;
        StateJson = !ReferenceEquals(stateInstance, EMPTY_OBJECT) ? Serializer.ToJson(stateInstance) : null;
        
        parametersInstance = null;
        stateInstance = null;
        strategyLogic = null;

        loaded = false;
    }

    private object DeserializeParameters(string? parametersJson) {
        if (parametersJson == null) {
            return EMPTY_OBJECT;
        }
        try {
            return DoDeserializeParameters(parametersJson);
        }
        catch (Exception e) {
            throw new InvalidUserArgumentException(
                $"Failed to instantiate Strategy {ParentTemplateVersion.Name}. " +
                $"Unable to deserialize parameters: '{parametersJson}'.",
                e
            );
        }
    }

    private object DeserializeState(string? stateJson) {
        if (stateJson == null) {
            return CreateDefaultState();
        }
        try {
            return DoDeserializeState(stateJson);
        }
        catch (Exception e) {
            throw new SystemArgumentException(
                $"Failed to instantiate Strategy {ParentTemplateVersion.Name}. " +
                $"Unable to deserialize state object: '{stateJson}'.",
                e
            );
        }
    }

    protected abstract IStrategyLogic DoCreateLogic();
    protected abstract object DoDeserializeParameters(string parametersJson);
    protected abstract object DoDeserializeState(string stateJson);
    protected abstract object CreateDefaultState();
    
    protected object ParseJson(string json, string typeName) {
        if (json == null) {
            throw new InvalidUserArgumentException($"Missing parameters for Parameterized Strategy {ParentTemplateVersion.Name}");
        }
        var parametersType = Type.GetType(typeName)!;
        return Serializer.FromJson<object>(json, parametersType)!;
    }
}