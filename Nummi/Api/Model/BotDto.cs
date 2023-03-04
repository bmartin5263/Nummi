using Nummi.Core.Domain.Crypto;

namespace Nummi.Api.Model; 

public class BotDto {
    public string? Id { get; set; }    
    public DateTimeOffset? CreatedAt { get; set; }    
    public DateTimeOffset? UpdatedAt { get; set; }    
    public DateTimeOffset? DeletedAt { get; set; }    
    public string? Name { get; set; }    
    public decimal? Funds { get; set; }    
    public TradingMode? Mode { get; set; }    
    public bool? InErrorState { get; set; }
    public BotActivationDto? CurrentActivation { get; set; }
    public List<BotActivationDto> ActivationHistory { get; set; } = new();
}