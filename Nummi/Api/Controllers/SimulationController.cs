using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.App.Simulations;
using Nummi.Core.Domain.Simulations;
using Nummi.Core.Domain.Strategies;
using Nummi.Core.Domain.User;

namespace Nummi.Api.Controllers; 

[Authorize]
[Route("api/simulation")]
[ApiController]
public class SimulationController : ControllerBase {

    private SimulateStrategyCommand SimulateStrategyCommand { get; }
    private GetSimulationsQuery GetSimulationsQuery { get; }
    private GetOneSimulationQuery GetOneSimulationQuery { get; }

    public SimulationController(
        SimulateStrategyCommand activateBotCommand, 
        GetSimulationsQuery getSimulationsQuery, 
        GetOneSimulationQuery getOneSimulationQuery
    ) {
        SimulateStrategyCommand = activateBotCommand;
        GetSimulationsQuery = getSimulationsQuery;
        GetOneSimulationQuery = getOneSimulationQuery;
    }

    /// <summary>
    /// Get all Simulations
    /// </summary>
    [Route("")]
    [HttpGet]
    public IEnumerable<SimulationDto> GetSimulations() {
        return GetSimulationsQuery.Execute()
            .Select(v => v.ToDto());
    }

    /// <summary>
    /// Get Simulation by Id
    /// </summary>
    [Route("{id}")]
    [HttpGet]
    public SimulationDto GetSimulation(string id) {
        return GetOneSimulationQuery.Execute(SimulationId.FromString(id)).ToDto();
    }
    
    public record SimulateStrategyParametersDto {
        public required string StrategyTemplateId { get; init; }
        public required DateTimeOffset StartTime { get; init; }
        public required DateTimeOffset EndTime { get; init; }
        public required decimal Funds { get; init; }
        public JsonDocument? StrategyJsonParameters { get; init; }
    }

    /// <summary>
    /// Create a new Simulation from a given Strategy Template
    /// </summary>
    [Route("")]
    [HttpPost]
    public SimulationDto SimulateStrategy([FromBody] SimulateStrategyParametersDto request) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return SimulateStrategyCommand.Execute(new SimulateStrategyParameters {
                UserId = IdentityId.FromString(userId),
                StrategyTemplateId = StrategyTemplateId.FromString(request.StrategyTemplateId),
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Funds = request.Funds,
                StrategyJsonParameters = request.StrategyJsonParameters
            })
            .ToDto();
    }
}