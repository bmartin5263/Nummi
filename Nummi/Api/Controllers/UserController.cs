using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.Domain.New.Queries;

namespace Nummi.Api.Controllers;

[Route("api/user")]
[ApiController, Authorize]
public class UserController : ControllerBase {
    
    private GetUserQuery GetUserQuery { get; }

    public UserController(GetUserQuery getUserQuery) {
        GetUserQuery = getUserQuery;
    }

    [Route("me")]
    [HttpGet]
    public NummiUserDto GetMe() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return GetUserQuery.Execute(userId)
            .ToDto();
    }
    
}