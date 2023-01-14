using System.Text.Json.Nodes;

namespace Nummi.Api.Model; 

public class StrategyParameters {
    public string? Type { get; set; }
    public JsonNode? Data { get; set; }
}