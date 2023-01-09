using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Nummi.Api.Model; 

public class AssignBotRequest {
    [Required]
    public string? BotId { get; set; }
}