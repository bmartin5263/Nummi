using System.Runtime.InteropServices;
using Alpaca.Markets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestWebApp.Domain;
using TestWebApp.Domain.Model;

namespace TestWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class TradeController : ControllerBase {

    private readonly ILogger<TradeController> _logger;
    private readonly AlpacaClient _alpacaClient;

    public TradeController(
        ILogger<TradeController> logger, 
        AlpacaClient alpacaClient 
    ) {
        _logger = logger;
        _alpacaClient = alpacaClient;
    }

    [HttpGet]
    public async Task<IAccount> GetAccountDetails() {
        return await _alpacaClient.GetAccountDetails();
    }
}