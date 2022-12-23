using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestWebApp.Domain;
using TestWebApp.Domain.Model;

namespace TestWebApp.Controllers;

[ApiController]
[Route("[controller]")]     // becomes WeatherForecast
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] SUMMARIES = 
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly TradeService _tradeService;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger, 
        TradeService tradeService
    ) {
        _logger = logger;
        _tradeService = tradeService;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get() {
        _tradeService.Hello();
        return Enumerable.Range(1, 5)
            .Select(i => new WeatherForecast {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(i)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = SUMMARIES[Random.Shared.Next(SUMMARIES.Length)]
            })
            .ToArray();
    }
}