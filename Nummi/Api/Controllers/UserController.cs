using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.App.Queries;
using Nummi.Core.App.User;
using Nummi.Core.Domain.User;

namespace Nummi.Api.Controllers;

[Route("api/user")]
[ApiController, Authorize]
public class UserController : ControllerBase {
    private GetUserQuery GetUserQuery { get; }
    private LoginCommand LoginCommand { get; }
    private RegisterCommand RegisterCommand { get; }

    public UserController(GetUserQuery getUserQuery, LoginCommand loginCommand, RegisterCommand registerCommand) {
        GetUserQuery = getUserQuery;
        LoginCommand = loginCommand;
        RegisterCommand = registerCommand;
    }

    /// <summary>
    /// Get logged in user's information
    /// </summary>
    [Route("me")]
    [HttpGet]
    public NummiUserDto GetMe() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return GetUserQuery.Execute(IdentityId.FromString(userId))
            .ToDto();
    }

    /// <summary>
    /// Login a User, returning a JWT
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommandParameters args) {
        var result = await LoginCommand.Execute(args);
        return Ok(result);
    }

    /// <summary>
    /// Create a new User
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommandParameters args) {
        var result = await RegisterCommand.Execute(args);
        if (result.Success) {
            return Ok(result);
        }
        return BadRequest(result);
    }
}