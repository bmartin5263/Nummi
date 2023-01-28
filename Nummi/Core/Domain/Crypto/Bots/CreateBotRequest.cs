using System.ComponentModel.DataAnnotations;
using Nummi.Core.Domain.Crypto.Strategies;

namespace Nummi.Core.Domain.Crypto.Bots; 

public class CreateBotRequest {
    [Required]
    public string Name { get; set; } = "";
    
    [Required]
    public TradingMode Mode { get; set; }
    
    public decimal? Funds { get; set; }
}