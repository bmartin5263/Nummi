using System.Text.Json.Serialization;
using Nummi.Core.Domain.Common;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Strategies; 

[JsonConverter(typeof(Serializer.AbstractTypeConverter<StrategyTemplate>))]
public class StrategyTemplate : Audited {
    public Ksuid Id { get; } = Ksuid.Generate();
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
    
    /// User Id that owns this Strategy Template 
    public Ksuid UserId { get; private set; }

    /// Human-readable Name
    public string Name { get; set; }
    
    /// Allows Users other than the owner (UserId) to use this strategy
    public bool IsPublic { get; set; }

    public List<StrategyTemplateVersion> Versions { get; } = new();
    
    public StrategyTemplate() {
        UserId = null!;
        Name = null!;
    }
    
    public StrategyTemplate(Ksuid id, Ksuid owningUserId, string name, StrategyTemplateVersion firstVersion) {
        Id = id;
        UserId = owningUserId;
        Name = name;
        Versions.Add(firstVersion);
    }
}

