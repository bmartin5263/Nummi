using Microsoft.AspNetCore.Mvc;
using TestWebApp.Api.Model;
using TestWebApp.Core.Database;
using TestWebApp.Core.Domain;
using TestWebApp.Core.Domain.Stocks;
using TestWebApp.Core.Domain.Stocks;
using TestWebApp.Core.Domain.Stocks.Data;
using TestWebApp.Core.Domain.Stocks.Ordering;
using TestWebApp.Core.Domain.User;
using TestWebApp.Core.External.Alpaca;

namespace TestWebApp.Api.Controllers;

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