using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.Domain.Crypto.Bots;
using Nummi.Core.Domain.Crypto.Bots.Thread;

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
    
    /// <summary>
    /// Clears the Error State for a given Bot
    /// </summary>
    [Route("{botId}/error")]
    [HttpDelete]
    public void ClearErrorState(string botId) {
        BotService.ClearErrorState(botId);
    }

    /// <summary>
    /// Assigns the given Bot to a thread for executing its trading strategy
    /// </summary>
    [Route("{botId}/activate")]
    [HttpPut]
    public BotThreadDetail ActivateBot(
        string botId
    ) {
        return BotService.ActivateBot(botId);
    }
    
    /// <summary>
    /// Removes a Bot from its running thread, ending its strategy run
    /// </summary>
    [Route("{botId}/deactivate")]
    [HttpDelete]
    public BotThreadDetail DeactivateBot(
        string botId
    ) {
        return BotService.DeactivateBot(botId);
    }
    
    /// <summary>
    /// Runs a Bot's strategy in simulation mode
    /// </summary>
    [Route("{botId}/simulation")]
    [HttpPost]
    public string RunSimulation(
        string botId,
        [FromBody] SimulationParameters parameters
    ) {
        return BotService.RunBotSimulation(botId, parameters);
    }
    
    /// <summary>
    /// Get details of a simulation run. May not be complete if the simulation hasn't finished
    /// </summary>
    [Route("simulation/{simulationId}")]
    [HttpPost]
    public SimulationDto GetSimulation(
        string simulationId
    ) {
        return BotService.GetSimulation(simulationId).ToDto();
    }
}