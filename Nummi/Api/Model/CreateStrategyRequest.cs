using System.Text.Json.Nodes;

namespace Nummi.Api.Model; 

public class CreateStrategyRequest {
    public string? StrategyName { get; set; } = default!;
    public JsonObject? Parameters { get; set; }
}