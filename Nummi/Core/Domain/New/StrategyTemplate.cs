using System.Text.Json.Serialization;
using Nummi.Core.Domain.Common;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New; 

[JsonConverter(typeof(Serializer.AbstractTypeConverter<StrategyTemplate>))]
public abstract class StrategyTemplate : Audited {
    public Ksuid Id { get; } = Ksuid.Generate();
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
    
    public string UserId { get; private set; }

    public string Name { get; set; }
    
    public ulong Version { get; set; }
    
    public TimeSpan Frequency { get; set; }
    
    public abstract bool AcceptsParameters { get; }

    protected StrategyTemplate() {
        UserId = null!;
        Name = null!;
    }
    
    protected StrategyTemplate(string owningUserId, string name, TimeSpan frequency, ulong version = 0) {
        UserId = owningUserId;
        Name = name;
        Frequency = frequency;
        Version = version;
    }

    public StrategyTemplate UpdateNewVersion(Action<StrategyTemplate> update) {
        var newVersion = CreateNewVersion();
        update(newVersion);
        return newVersion;
    }
    
    public abstract Strategy Instantiate(string? jsonParameters);
    
    protected abstract StrategyTemplate CreateNewVersion();
}

