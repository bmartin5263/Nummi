using KSUID;
using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.Domain.Crypto.Bot;
using Nummi.Core.Domain.Crypto.Bot.Execution;
using Nummi.Core.Domain.Crypto.Bot.Strategy;

namespace Nummi.Api.Controllers; 

[Route("api")]
[ApiController]
public class BotController : ControllerBase {

    private BotService botService;
    private BotExecutor botExecutor;

    public BotController(BotService botService, BotExecutor botExecutor) {
        this.botService = botService;
        this.botExecutor = botExecutor;
    }

    [Route("bot")]
    [HttpPost]
    public StockBotDto CreateBot(CreateBotRequest request) {
        return botService.CreateBot(request).ToDto();
    }
    
    [Route("bot/{id}")]
    [HttpGet]
    public StockBotDto GetBot(string id) {
        return botService.GetBot(Ksuid.FromString(id)).ToDto();
    }
    
    [Route("bot/{id}/strategy")]
    [HttpPost]
    public StockBotDto SetBotStrategy(string id, ChangeStrategyRequest request) {
        return botService.ChangeBotStrategy(Ksuid.FromString(id), request).ToDto();
    }
    
    [Route("threads")]
    [HttpGet]
    public BotThreadsOverview GetThreads() {
        return botExecutor.GetOverview();
    }
    
    [Route("threads/{threadId}/bot")]
    [HttpPost]
    public void AssignBotToThread(
        uint threadId,
        [FromBody] AssignBotRequest request
    ) {
        var botId = Ksuid.FromString(request.BotId);
        botService.ValidateId(botId);
        
        var thread = botExecutor.GetThread(threadId);
        thread.RegisterBot(botId);
    }
    
    [Route("threads/{threadId}/bot")]
    [HttpDelete]
    public void RemoveBotFromThread(
        uint threadId
    ) {
        var thread = botExecutor.GetThread(threadId);
        thread.DeregisterBot();
    }
    
}