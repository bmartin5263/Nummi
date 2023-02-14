using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.Domain.New.Commands;
using Nummi.Core.Util;

namespace Nummi.Api.Controllers; 

[Route("api/strategy-template")]
[ApiController]
public class StrategyTemplateController : ControllerBase {

    private CreateBotCommand CreateBotCommand { get; }
    private SimulateStrategyCommand SimulateStrategyCommand { get; }
    private DeactivateBotCommand DeactivateBotCommand { get; }

    public StrategyTemplateController(CreateBotCommand createBotCommand, SimulateStrategyCommand activateBotCommand, DeactivateBotCommand deactivateBotCommand) {
        CreateBotCommand = createBotCommand;
        SimulateStrategyCommand = activateBotCommand;
        DeactivateBotCommand = deactivateBotCommand;
    }

    /// <summary>
    /// Create a new Bot instance
    /// </summary>
    [Route("{strategyTemplateId}/simulate")]
    [HttpPost]
    public SimulationDto SimulateStrategy(string strategyTemplateId, [FromBody] SimulateStrategyParameters request) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return SimulateStrategyCommand.Execute(userId, strategyTemplateId.ToKsuid(), request)
            .ToDto();
    }
}