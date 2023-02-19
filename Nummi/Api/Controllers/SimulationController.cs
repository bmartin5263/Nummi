using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.Domain.New.Commands;
using Nummi.Core.Domain.New.Queries;

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
        return GetOneSimulationQuery.Execute(id).ToDto();
    }

    /// <summary>
    /// Create a new Simulation from a given Strategy Template
    /// </summary>
    [Route("")]
    [HttpPost]
    public SimulationDto SimulateStrategy([FromBody] SimulateStrategyParameters request) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return SimulateStrategyCommand.Execute(userId, request)
            .ToDto();
    }
}