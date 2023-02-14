using Microsoft.AspNetCore.Identity;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.New;

namespace Nummi.Core.Domain.User;

public class NummiUser : IdentityUser, Audited {

    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
    
    public string? AlpacaKeyId { get; set; }
    
    public string? AlpacaSecret { get; set; }

    public List<Bot> Bots { get; } = new();

    public List<StrategyTemplate> StrategyTemplates { get; } = new();

    public List<Simulation> Simulations { get; } = new();

}