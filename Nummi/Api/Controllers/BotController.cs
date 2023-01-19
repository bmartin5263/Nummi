using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.Domain.Crypto.Bots;

namespace Nummi.Api.Controllers; 

[Route("api/bot")]
[ApiController]
public class BotController : ControllerBase {

    private BotService BotService {get; set;}

    public BotController(BotService botService) {
        BotService = botService;
    }

    /// <summary>
    /// Create a new Bot instance
    /// </summary>
    [Route("")]
    [HttpPost]
    public BotDto CreateBot(CreateBotRequest request) {
        return BotService
            .CreateBot(request)
            .ToDto();
    }
    
    /// <summary>
    /// Get a Bot by Id
    /// </summary>
    [Route("{botId}")]
    [HttpGet]
    public BotDto GetBotById(string botId) {
        return BotService
            .GetBotById(botId)
            .ToDto();
    }

    /// <summary>
    /// Delete a Bot by Id
    /// </summary>
    [Route("{botId}")]
    [HttpDelete]
    public void DeleteBotById(string botId) {
        BotService.DeleteBotById(botId);
    }
    
    /// <summary>
    /// Get all Bots in the DB
    /// </summary>
    [HttpGet]
    public BotFilterResponse GetAllBots() {
        var bots = BotService
            .GetBots()
            .Select(v => v.ToDto())
            .ToList();
        
        return new BotFilterResponse(bots);
    }
    
    /// <summary>
    /// Assign a Trading Strategy to a given Bot
    /// </summary>
    [Route("{botId}/strategy/{strategyId}")]
    [HttpPatch]
    public BotDto SetBotStrategy(string botId, string strategyId) {
        return BotService
            .SetBotStrategy(botId, strategyId)
            .ToDto();
    }
    
    // [Route("{botId}/strategy")]
    // [HttpPost]
    // public BotDto RunBotStrategy(string botId) {
    //     return BotService
    //         .RunBotStrategy(botId)
    //         .ToDto();
    // }
    //
    // [Route("{strategyId}/strategy2")]
    // [HttpPost]
    // public StrategyDto RunBotStrategy2(string strategyId) {
    //     return BotService
    //         .RunBotStrategy2(strategyId)
    //         .ToDto();
    // }
    
    /// <summary>
    /// Clears the Error State for a given Bot
    /// </summary>
    [Route("{botId}/error")]
    [HttpDelete]
    public void ClearErrorState(string botId) {
        BotService.ClearErrorState(botId);
    }

}