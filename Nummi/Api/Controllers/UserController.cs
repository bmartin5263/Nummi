using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Nummi.Api.Model;
using Nummi.Core.App.Queries;
using Nummi.Core.Config;
using Nummi.Core.Domain.User;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Nummi.Api.Controllers;

public class Response {
    public string? Status { get; set; }
    public string? Message { get; set; }
}

public class RegisterRequest {
    [Required(ErrorMessage = "User Name is required")]
    public string? Username { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}

public class LoginRequest {
    [Required(ErrorMessage = "User Name is required")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}

[Route("api/user")]
[ApiController, Authorize]
public class UserController : ControllerBase {
    private GetUserQuery GetUserQuery { get; }
    private UserManager<NummiUser> UserManager { get; }
    private RoleManager<NummiRole> RoleManager { get; }
    private IConfiguration Configuration { get; }

    public UserController(GetUserQuery getUserQuery, UserManager<NummiUser> userManager,
        RoleManager<NummiRole> roleManager, IConfiguration configuration) {
        GetUserQuery = getUserQuery;
        UserManager = userManager;
        RoleManager = roleManager;
        Configuration = configuration;
    }

    [Route("me")]
    [HttpGet]
    public NummiUserDto GetMe() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return GetUserQuery.Execute(IdentityId.FromString(userId))
            .ToDto();
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model) {
        var user = await UserManager.FindByNameAsync(model.Username!);
        if (user == null || !await UserManager.CheckPasswordAsync(user, model.Password!)) {
            return Unauthorized();
        }
        
        var authClaims = new List<Claim> {
            new(ClaimTypes.Name, user.UserName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var userRoles = await UserManager.GetRolesAsync(user);
        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var token = GetToken(authClaims);
        return Ok(new {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model) {
        var userExists = await UserManager.FindByNameAsync(model.Username!);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "User already exists!" });

        NummiUser user = new() {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username!
        };
        var result = await UserManager.CreateAsync(user, model.Password!);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response
                    { Status = "Error", Message = "User creation failed! Please check user details and try again." });

        return Ok(new Response { Status = "Success", Message = "User created successfully!" });
    }

    [HttpPost]
    [Route("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequest model) {
        var userExists = await UserManager.FindByNameAsync(model.Username!);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "User already exists!" });

        NummiUser user = new() {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username!
        };
        var result = await UserManager.CreateAsync(user, model.Password!);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response
                    { Status = "Error", Message = "User creation failed! Please check user details and try again." });

        await UserManager.AddToRoleAsync(user, RoleName.Admin.ToString());
        await UserManager.AddToRoleAsync(user, RoleName.User.ToString());

        return Ok(new Response { Status = "Success", Message = "User created successfully!" });
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims) {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]!));

        var token = new JwtSecurityToken(
            issuer: Configuration["JWT:Issuer"],
            audience: Configuration["JWT:Audience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}