using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.App.Simulations;
using Nummi.Core.App.Strategies;

namespace Nummi.Api.Controllers; 

[Authorize]
[Route("api/strategy-template")]
[ApiController]
public class StrategyTemplateController : ControllerBase {

    private SimulateStrategyCommand SimulateStrategyCommand { get; }
    private GetStrategyTemplatesQuery GetStrategyTemplatesQuery { get; }
    private InitializeBuiltinStrategiesCommand InitializeBuiltinStrategiesCommand { get; }

    public StrategyTemplateController(
        SimulateStrategyCommand activateBotCommand, 
        GetStrategyTemplatesQuery getStrategyTemplatesQuery, 
        InitializeBuiltinStrategiesCommand initializeBuiltinStrategiesCommand
    ) {
        SimulateStrategyCommand = activateBotCommand;
        GetStrategyTemplatesQuery = getStrategyTemplatesQuery;
        InitializeBuiltinStrategiesCommand = initializeBuiltinStrategiesCommand;
    }

    /// <summary>
    /// Get all Strategy Templates
    /// </summary>
    [Route("")]
    [HttpGet]
    [AllowAnonymous]
    public IEnumerable<StrategyTemplateDto> GetStrategyTemplates() {
        return GetStrategyTemplatesQuery.Execute()
            .Select(v => v.ToDto());
    }
}