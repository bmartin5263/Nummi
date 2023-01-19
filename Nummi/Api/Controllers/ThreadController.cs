using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.Domain.Crypto.Bots;
using Nummi.Core.Domain.Crypto.Bots.Execution;

namespace Nummi.Api.Controllers; 

[Route("api/threads")]
[ApiController]
public class ThreadController : ControllerBase {

    private BotService BotService {get; }
    private BotExecutor BotExecutor {get; }

    public ThreadController(BotService botService, BotExecutor botExecutor) {
        BotService = botService;
        BotExecutor = botExecutor;
    }

    /// <summary>
    /// Return all running Threads
    /// </summary>
    [Route("")]
    [HttpGet]
    public BotThreadsOverview GetThreads() {
        return BotExecutor.GetThreads();
    }
    
    /// <summary>
    /// Assign a Bot to a given Thread
    /// </summary>
    [Route("{threadId}/bot")]
    [HttpPut]
    public void AssignBotToThread(
        uint threadId,
        [FromBody] AssignBotRequest request
    ) {
        var botId = request.BotId!;
        BotService.ValidateId(botId);
        
        var thread = BotExecutor.GetThread(threadId);
        thread.RegisterBot(botId);
    }
    
    /// <summary>
    /// Remove a Thread's Bot
    /// </summary>
    [Route("{threadId}/bot")]
    [HttpDelete]
    public void RemoveBotFromThread(
        uint threadId
    ) {
        var thread = BotExecutor.GetThread(threadId);
        thread.DeregisterBot();
    }
    
}