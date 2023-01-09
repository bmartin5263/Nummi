using KSUID;
using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.Domain.Stocks.Bot;
using Nummi.Core.Domain.Stocks.Bot.Execution;

namespace Nummi.Api.Controllers; 

[Route("api/bot")]
[ApiController]
public class BotController : ControllerBase {

    private BotService botService;
    private BotExecutor botExecutor;

    public BotController(BotService botService, BotExecutor botExecutor) {
        this.botService = botService;
        this.botExecutor = botExecutor;
    }

    [HttpPost]
    public StockBotDto CreateBot(CreateBotRequest request) {
        return botService.CreateBot(request).ToDto();
    }
    
    [Route("{id}")]
    [HttpGet]
    public StockBotDto GetBot(string id) {
        return botService.GetBot(Ksuid.FromString(id)).ToDto();
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