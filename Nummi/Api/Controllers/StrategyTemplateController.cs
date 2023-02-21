using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.Config;
using Nummi.Core.Domain.New.Commands;
using Nummi.Core.Domain.New.Queries;
using Nummi.Core.Domain.Strategies;

namespace Nummi.Api.Controllers; 

[Authorize]
[Route("api/strategy-template")]
[ApiController]
public class StrategyTemplateController : ControllerBase {

    private SimulateStrategyCommand SimulateStrategyCommand { get; }
    private GetStrategyTemplatesQuery GetStrategyTemplatesQuery { get; }
    private ReInitializeBuiltinStrategiesCommand ReInitializeBuiltinStrategiesCommand { get; }

    public StrategyTemplateController(
        SimulateStrategyCommand activateBotCommand, 
        GetStrategyTemplatesQuery getStrategyTemplatesQuery, 
        ReInitializeBuiltinStrategiesCommand reInitializeBuiltinStrategiesCommand
    ) {
        SimulateStrategyCommand = activateBotCommand;
        GetStrategyTemplatesQuery = getStrategyTemplatesQuery;
        ReInitializeBuiltinStrategiesCommand = reInitializeBuiltinStrategiesCommand;
    }

    /// <summary>
    /// Get all Strategy Templates
    /// </summary>
    [Route("")]
    [HttpGet]
    public IEnumerable<StrategyTemplateDto> GetStrategyTemplates() {
        return GetStrategyTemplatesQuery.Execute()
            .Select(v => v.ToDto());
    }

    /// <summary>
    /// Get all Strategy Templates
    /// </summary>
    [Route("init")]
    [HttpPost]
    public void InitializeStrategyTemplates() {
        ReInitializeBuiltinStrategiesCommand.Execute();
    }
}