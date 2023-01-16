using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.Domain.Crypto.Strategies;

namespace Nummi.Api.Controllers; 

[Route("api/strategy")]
[ApiController]
public class StrategyController : ControllerBase {

    private StrategyService StrategyService { get; }

    public StrategyController(StrategyService strategyService) {
        StrategyService = strategyService;
    }
    
    [Route("")]
    [HttpPost]
    public StrategyDto CreateStrategy([FromBody] CreateStrategyRequest request) {
        return StrategyService
            .CreateStrategy(request.StrategyName!, request.Parameters)
            .ToDto();
    }
    
    [Route("{strategyId}")]
    [HttpPut]
    public StrategyDto UpdateStrategyParameters(string strategyId, [FromBody] JsonNode parameters) {
        return StrategyService
            .UpdateStrategyParameters(strategyId, parameters)
            .ToDto();
    }
    
    [Route("")]
    [HttpGet]
    public StrategyFilterResponse GetStrategies() {
        var strategies = StrategyService
            .GetStrategies()
            .Select(v => v.ToDto())
            .ToList();

        return new StrategyFilterResponse(strategies);
    }
    
    [Route("{strategyId}")]
    [HttpGet]
    public StrategyDto GetStrategyById(string strategyId) {
        return StrategyService
            .GetStrategyById(strategyId)
            .ToDto();
    }
    
    [Route("{strategyId}/error")]
    [HttpDelete]
    public void ClearErrorState(string strategyId) {
        StrategyService.ClearErrorState(strategyId);
    }
}