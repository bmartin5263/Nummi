using System.Text.Json.Serialization;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.New;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Strategies;

[JsonConverter(typeof(Serializer.AbstractTypeConverter<Strategy>))]
public abstract class Strategy : Audited {
    public Ksuid Id { get; } = Ksuid.Generate();

    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }

    public StrategyTemplateVersion ParentTemplate { get; }

    public TimeSpan Frequency { get; }
    
    public string? ParametersJson { get; set; }

    public string? StateJson { get; private set; }
    
    public List<StrategyLog> Logs { get; } = new();
    
    private object? stateInstance;
    private object? parametersInstance;
    private IStrategyLogic? strategyLogic;

    protected Strategy() {
        ParentTemplate = null!;
    }

    protected Strategy(StrategyTemplateVersion parentTemplate) {
        ParentTemplate = parentTemplate;
        Frequency = parentTemplate.Frequency;
    }

    public void Initialize(ITradingContext ctx) {
        Load();
        strategyLogic!.Initialize(ctx);
        Save();
    }

    public void CheckForTrades(ITradingContext ctx) {
        Load();
        strategyLogic!.CheckForTrades(ctx);
        Save();
    }

    public void Load() {
        if (parametersInstance == null && ParametersJson != null) {
            parametersInstance = DeserializeParameters(ParametersJson);
        }
        if (stateInstance == null && StateJson != null) {
            stateInstance = DeserializeState(StateJson);
        }
        strategyLogic ??= DoCreateLogic();
    }

    public void Save() {
        if (parametersInstance != null) {
            ParametersJson = Serializer.ToJson(parametersInstance);
        }
        if (stateInstance != null) {
            StateJson = Serializer.ToJson(stateInstance);
        }
    }

    private object DeserializeParameters(string parametersJson) {
        try {
            return DoDeserializeParameters(parametersJson);
        }
        catch (Exception e) {
            throw new InvalidUserArgumentException(
                $"Failed to instantiate Strategy {ParentTemplate.Name}. " +
                $"Unable to deserialize parameters: '{parametersJson}'.",
                e
            );
        }
    }

    private object DeserializeState(string stateJson) {
        try {
            return DoDeserializeState(stateJson);
        }
        catch (Exception e) {
            throw new SystemArgumentException(
                $"Failed to instantiate Strategy {ParentTemplate.Name}. " +
                $"Unable to deserialize state object: '{stateJson}'.",
                e
            );
        }
    }

    protected abstract IStrategyLogic DoCreateLogic();
    protected abstract object DoDeserializeParameters(string parametersJson);
    protected abstract object DoDeserializeState(string stateJson);
    
    protected object ParseJson(string json, string typeName) {
        if (json == null) {
            throw new InvalidUserArgumentException($"Missing parameters for Parameterized Strategy {ParentTemplate.Name}");
        }
        var parametersType = Type.GetType(typeName)!;
        return Serializer.FromJson<object>(json, parametersType)!;
    }
}