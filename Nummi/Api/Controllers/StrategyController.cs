using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.Domain.Crypto.Trading.Strategy;

namespace Nummi.Api.Controllers; 

[Route("api/strategy")]
[ApiController]
public class StrategyController : ControllerBase {

    private readonly StrategyService strategyService;

    public StrategyController(StrategyService strategyService) {
        this.strategyService = strategyService;
    }
    
    [HttpPost]
    public StrategyDto CreateStrategy([FromBody] CreateStrategyRequest request) {
        return strategyService.CreateStrategy(request.StrategyName!, request.Parameters).ToDto();
    }
}