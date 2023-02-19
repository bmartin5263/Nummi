namespace Nummi.Api.Model; 

public class NummiUserDto {
    public string? Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public IList<BotDto> Bots { get; set; } = new List<BotDto>();
    public IList<SimulationDto> Simulations { get; set; } = new List<SimulationDto>();
}