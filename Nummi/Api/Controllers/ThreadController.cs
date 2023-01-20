using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.Domain.Crypto.Bots;
using Nummi.Core.Domain.Crypto.Bots.Thread;

namespace Nummi.Api.Controllers; 

[Route("api/threads")]
[ApiController]
public class ThreadController : ControllerBase {

    private BotService BotService {get; }
    private BotThreadSpawner BotThreadSpawner {get; }

    public ThreadController(BotService botService, BotThreadSpawner botThreadSpawner) {
        BotService = botService;
        BotThreadSpawner = botThreadSpawner;
    }

    /// <summary>
    /// Return all running Threads
    /// </summary>
    [Route("")]
    [HttpGet]
    public BotThreadsOverview GetThreads() {
        return BotThreadSpawner.GetThreads();
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
        
        var thread = BotThreadSpawner.GetThread(threadId);
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
        var thread = BotThreadSpawner.GetThread(threadId);
        thread.DeregisterBot();
    }
    
}