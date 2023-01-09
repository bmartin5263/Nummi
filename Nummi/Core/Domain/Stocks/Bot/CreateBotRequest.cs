using System.ComponentModel.DataAnnotations;

namespace Nummi.Core.Domain.Stocks.Bot; 

public class CreateBotRequest {
    [Required]
    public string? Name { get; set; }
}