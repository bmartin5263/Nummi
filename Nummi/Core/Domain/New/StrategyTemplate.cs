using System.Text.Json.Serialization;
using KSUID;
using Nummi.Core.Domain.Common;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New; 

[JsonConverter(typeof(Serializer.AbstractTypeConverter<StrategyTemplate>))]
public abstract class StrategyTemplate : Audited {
    public Ksuid Id { get; } = Ksuid.Generate();
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }

    public string Name { get; set; }
    
    public TimeSpan Frequency { get; set; }
    
    protected StrategyTemplate(string name) {
        Name = name;
        Frequency = TimeSpan.FromMinutes(1);
    }

    public abstract bool AcceptsParameters { get; }
    public abstract Strategy Instantiate(string? jsonParameters);
}

