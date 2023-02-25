using System.ComponentModel.DataAnnotations;

namespace Nummi.Api.Model; 

public class AssignBotRequest {
    [Required]
    public string? BotId { get; set; }
}