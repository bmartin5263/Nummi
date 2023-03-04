using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.User;

namespace Nummi.Core.Domain.Strategies;

public readonly record struct StrategyTemplateId(Guid Value) {
    public override string ToString() => Value.ToString("N");
    public static StrategyTemplateId Generate() => new(Guid.NewGuid());
    public static StrategyTemplateId FromGuid(Guid id) => new(id);
    public static StrategyTemplateId FromString(string s) => new(Guid.ParseExact(s, "N"));
}

public class StrategyTemplate : Audited {
    public StrategyTemplateId Id { get; } = StrategyTemplateId.Generate();
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
    
    /// User Id that owns this Strategy Template 
    public IdentityId UserId { get; private set; }

    /// Human-readable Name
    public string Name { get; set; }
    
    /// Allows Users other than the owner (UserId) to use this strategy
    public bool IsPublic { get; set; }

    public List<StrategyTemplateVersion> Versions { get; } = new();
    
    public StrategyTemplate() {
        Name = null!;
    }
    
    public StrategyTemplate(StrategyTemplateId id, IdentityId owningUserId, string name, StrategyTemplateVersion firstVersion) {
        Id = id;
        UserId = owningUserId;
        Name = name;
        Versions.Add(firstVersion);
    }
}

