using Microsoft.AspNetCore.Mvc;
using Nummi.Core.Database;
using Nummi.Core.Domain.Stocks.Data;
using Nummi.Core.Domain.Stocks.Ordering;
using Nummi.Core.Domain.User;
using Nummi.Core.External.Alpaca;
using Nummi.Api.Model;
using Nummi.Core.Domain;
using Nummi.Core.Domain.Stocks;

namespace Nummi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase {

    private readonly ILogger<UserController> logger;
    private readonly AppDb appDb;

    public UserController(
        ILogger<UserController> logger, 
        IAlpacaClient alpacaClient,
        OrderService orderService,
        MarketDataService marketDataService, 
        AppDb appDb
    ) {
        this.logger = logger;
        this.appDb = appDb;
        logger.LogInformation("New UserController");
        logger.LogInformation(string.Empty + this.appDb.ContextId);
    }

    /// <summary>
    /// Get User Details
    /// </summary>
    [HttpGet]
    [Route("{id}")]
    public HashSet<User> GetSnapshot(string id) {
        return appDb.Users.ToHashSet();
    }
}