using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.Domain.New.Commands;
using Nummi.Core.Domain.New.Queries;

namespace Nummi.Api.Controllers; 

[Authorize]
[Route("api/strategy-template")]
[ApiController]
public class StrategyTemplateController : ControllerBase {

    private SimulateStrategyCommand SimulateStrategyCommand { get; }
    private GetStrategyTemplatesQuery GetStrategyTemplatesQuery { get; }

    public StrategyTemplateController(
        SimulateStrategyCommand activateBotCommand, 
        GetStrategyTemplatesQuery getStrategyTemplatesQuery
    ) {
        SimulateStrategyCommand = activateBotCommand;
        GetStrategyTemplatesQuery = getStrategyTemplatesQuery;
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
}