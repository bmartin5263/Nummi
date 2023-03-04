namespace Nummi.Api.Model; 

public class NummiUserDto {
    public string? Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public List<BotDto> Bots { get; set; } = new();
    public List<SimulationDto> Simulations { get; set; } = new();
    public List<StrategyTemplateDto> StrategyTemplates { get; set; } = new();
}