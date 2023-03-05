using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.App.Commands;
using Nummi.Core.Domain.Bots;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.Domain.Strategies;
using Nummi.Core.Domain.User;

namespace Nummi.Api.Controllers; 

[Authorize]
[Route("api/bot")]
[ApiController]
public class BotController : ControllerBase {

    private CreateBotCommand CreateBotCommand { get; }
    private ActivateBotCommand ActivateBotCommand { get; }
    private DeactivateBotCommand DeactivateBotCommand { get; }
    private ReactivateBotCommand ReactivateBotCommand { get; }

    public BotController(CreateBotCommand createBotCommand, ActivateBotCommand activateBotCommand, DeactivateBotCommand deactivateBotCommand, ReactivateBotCommand reactivateBotCommand) {
        CreateBotCommand = createBotCommand;
        ActivateBotCommand = activateBotCommand;
        DeactivateBotCommand = deactivateBotCommand;
        ReactivateBotCommand = reactivateBotCommand;
    }
    
    public record CreateBotParametersDto {
        public required string Name { get; init; }
        public required TradingMode Mode { get; init; }
        public decimal? Funds { get; init; }
        public string? UserId { get; init; }
    }

    /// <summary>
    /// Create a new Bot instance
    /// </summary>
    [Route("")]
    [HttpPost]
    public BotDto CreateBot([FromBody] CreateBotParametersDto request) {
        var userId = request.UserId ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return CreateBotCommand.Execute(new CreateBotParameters {
                UserId = IdentityId.FromString(userId),
                Name = request.Name,
                Mode = request.Mode,
                Funds = request.Funds
            })
            .ToDto();
    }

    public record ActivateBotParametersDto {
        public required string StrategyTemplateId { get; init; }
        public JsonDocument? StrategyJsonParameters { get; init; }
    }
    
    /// <summary>
    /// Activate a Bot, causing it to start getting regularly scheduled for trading
    /// </summary>
    [Route("{botId}/activation")]
    [HttpPost]
    public BotActivationDto ActivateBot(string botId, [FromBody] ActivateBotParametersDto request) {
        return ActivateBotCommand.Execute(new ActivateBotParameters {
                BotId = BotId.FromString(botId),
                StrategyTemplateId = StrategyTemplateId.FromString(request.StrategyTemplateId),
                StrategyJsonParameters = request.StrategyJsonParameters
            })
            .ToDto();
    }
    
    /// <summary>
    /// Deactivate a Bot, ending its trading scheduling
    /// </summary>
    [Route("{botId}/activation")]
    [HttpDelete]
    public void DeactivateBot(string botId) { 
        DeactivateBotCommand.Execute(BotId.FromString(botId));
    }
    
    /// <summary>
    /// Clear a Bot's Error State and reactivate it
    /// </summary>
    [Route("{botId}/reactivate")]
    [HttpPost]
    public void ClearBotErrorState(string botId) { 
        ReactivateBotCommand.Execute(BotId.FromString(botId));
    }
}