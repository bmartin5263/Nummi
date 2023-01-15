using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.Domain.Crypto.Bot;

namespace Nummi.Api.Controllers; 

[Route("api/bot")]
[ApiController]
public class BotController : ControllerBase {

    private BotService BotService {get; set;}

    public BotController(BotService botService) {
        BotService = botService;
    }

    [Route("")]
    [HttpPost]
    public StockBotDto CreateBot(CreateBotRequest request) {
        return BotService
            .CreateBot(request)
            .ToDto();
    }
    
    [Route("{botId}")]
    [HttpGet]
    public StockBotDto GetBotById(string botId) {
        return BotService
            .GetBotById(botId)
            .ToDto();
    }
    
    [Route("{botId}")]
    [HttpDelete]
    public void DeleteBotById(string botId) {
        BotService.DeleteBotById(botId);
    }
    
    [HttpGet]
    public BotFilterResponse GetAllBots() {
        var bots = BotService
            .GetBots()
            .Select(v => v.ToDto())
            .ToList();
        
        return new BotFilterResponse(bots);
    }
    
    [Route("{botId}/strategy/{strategyId}")]
    [HttpPatch]
    public StockBotDto SetBotStrategy(string botId, string strategyId) {
        return BotService
            .SetBotStrategy(botId, strategyId)
            .ToDto();
    }
    
    [Route("{botId}/strategy")]
    [HttpPost]
    public StockBotDto RunBotStrategy(string botId) {
        return BotService
            .RunBotStrategy(botId)
            .ToDto();
    }
    
    [Route("{strategyId}/strategy2")]
    [HttpPost]
    public StrategyDto RunBotStrategy2(string strategyId) {
        return BotService
            .RunBotStrategy2(strategyId)
            .ToDto();
    }

}