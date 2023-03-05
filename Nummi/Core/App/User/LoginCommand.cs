using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Nummi.Core.Bridge;

namespace Nummi.Core.App.User; 

public record LoginCommandParameters {
    public required string Username { get; init; }
    public required string Password { get; init; }
}

public record LoginResponse {
    public required Jwt Token { get; init; }
    public required DateTimeOffset CurrentTime { get; init; }
    public required DateTimeOffset ExpiresAt { get; init; }
}

public class LoginCommand {
    private INummiUserManager UserManager { get; }
    private IJwtMinter JwtMinter { get; }

    public LoginCommand(INummiUserManager userManager, IJwtMinter jwtMinter) {
        UserManager = userManager;
        JwtMinter = jwtMinter;
    }

    public async Task<LoginResponse> Execute(LoginCommandParameters args) {
        var user = await UserManager.LoginAsync(args.Username, args.Password);
        var userRoles = UserManager.GetRolesAsync(user);
        var authClaims = new List<Claim> {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.NameIdentifier, user.Id.Value.ToString("N")),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
        };

        var awaitedRoles = await userRoles;
        authClaims.AddRange(awaitedRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var token = JwtMinter.MintToken(authClaims);
        return new LoginResponse {
            CurrentTime = DateTimeOffset.Now,
            ExpiresAt = DateTimeOffset.Now + TimeSpan.FromDays(3),
            Token = token
        };
    }
}