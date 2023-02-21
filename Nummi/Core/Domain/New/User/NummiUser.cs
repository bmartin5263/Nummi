using Microsoft.AspNetCore.Identity;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Strategies;

namespace Nummi.Core.Domain.New.User;

public class NummiUser : IdentityUser<Ksuid>, Audited {

    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
    
    public string? AlpacaPaperId { get; set; }
    
    public string? AlpacaPaperKey { get; set; }
    
    public string? AlpacaLiveId { get; set; }
    
    public string? AlpacaLiveKey { get; set; }

    public List<Bot> Bots { get; } = new();

    public List<StrategyTemplate> StrategyTemplates { get; } = new();

    public List<Simulation> Simulations { get; } = new();

}