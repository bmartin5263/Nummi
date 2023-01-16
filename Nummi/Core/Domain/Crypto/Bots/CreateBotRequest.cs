using System.ComponentModel.DataAnnotations;

namespace Nummi.Core.Domain.Crypto.Bots; 

public class CreateBotRequest {
    [Required]
    public string Name { get; set; } = "";
    public decimal? Funds { get; set; }
}