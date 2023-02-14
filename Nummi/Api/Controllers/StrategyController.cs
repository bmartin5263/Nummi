// using System.Text.Json.Nodes;
// using Microsoft.AspNetCore.Mvc;
// using Nummi.Api.Model;
// using Nummi.Core.Domain.Crypto.Strategies;
//
// namespace Nummi.Api.Controllers; 
//
// [Route("api/strategy")]
// [ApiController]
// public class StrategyController : ControllerBase {
//
//     private StrategyService StrategyService { get; }
//
//     public StrategyController(StrategyService strategyService) {
//         StrategyService = strategyService;
//     }
//     
//     /// <summary>
//     /// Create a new instance of a given strategy with supplied parameters
//     /// </summary>
//     [Route("")]
//     [HttpPost]
//     public StrategyDto CreateStrategy([FromBody] CreateStrategyRequest request) {
//         return StrategyService
//             .CreateStrategy(request.StrategyName!, request.Parameters)
//             .ToDto();
//     }
//     
//     /// <summary>
//     /// Update a given Strategy's parameters
//     /// </summary>
//     [Route("{strategyId}")]
//     [HttpPut]
//     public StrategyDto UpdateStrategyParameters(string strategyId, [FromBody] JsonNode parameters) {
//         return StrategyService
//             .UpdateStrategyParameters(strategyId, parameters)
//             .ToDto();
//     }
//     
//     /// <summary>
//     /// Get all strategies in the DB
//     /// </summary>
//     [Route("")]
//     [HttpGet]
//     public StrategyFilterResponse GetStrategies() {
//         var strategies = StrategyService
//             .GetStrategies()
//             .Select(v => v.ToDto())
//             .ToList();
//
//         return new StrategyFilterResponse(strategies);
//     }
//     
//     /// <summary>
//     /// Get a Strategy by its Id
//     /// </summary>
//     [Route("{strategyId}")]
//     [HttpGet]
//     public StrategyDto GetStrategyById(string strategyId) {
//         return StrategyService
//             .GetStrategyById(strategyId)
//             .ToDto();
//     }
// }