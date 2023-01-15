using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.Domain.Crypto.Trading.Strategy;

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
    
    [Route("")]
    [HttpGet]
    public StrategyFilterResponse GetStrategies() {
        var strategies = StrategyService.GetStrategies()
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
}