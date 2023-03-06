using System.Collections.Immutable;
using Nummi.Core.Bridge;
using Nummi.Core.Domain.User;
using Nummi.Core.Exceptions;

namespace Nummi.Core.App.User;

public record RegisterCommandParameters(string Username, string Email, string Password);

public record RegisterResponse {
    public required bool Success { get; init; }
    public NummiUser? User { get; init; }
    public IList<string> PasswordErrors { get; init; } = ImmutableList<string>.Empty;

    public static RegisterResponse Succeeded(NummiUser user) {
        return new RegisterResponse { Success = true, User = user };
    }
    
    public static RegisterResponse Failed(IList<string> passwordErrors) {
        return new RegisterResponse { Success = false, PasswordErrors = passwordErrors};
    }
}

public class RegisterCommand {
    private INummiUserManager UserManager { get; }
    private IJwtMinter JwtMinter { get; }

    public RegisterCommand(INummiUserManager userManager, IJwtMinter jwtMinter) {
        UserManager = userManager;
        JwtMinter = jwtMinter;
    }

    public async Task<RegisterResponse> ExecuteAsync(RegisterCommandParameters args) {
        var userExists = UserManager.UserExists(args.Username);
        if (userExists) {
            throw new InvalidUserArgumentException($"Username {args.Username} is already taken");
        }

        var emailExists = UserManager.EmailExists(args.Email);
        if (emailExists) {
            throw new InvalidUserArgumentException($"Email {args.Email} is already taken");
        }
        
        var user = new NummiUser {
            Email = args.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = args.Username
        };
        var result = await UserManager.CreateUserAsync(user, args.Password);
        
        if (result.Succeeded) {
            return RegisterResponse.Succeeded(user);
        }
        
        return RegisterResponse.Failed(result.Errors
            .Where(e => e.Code.StartsWith("Password"))
            .Select(e => e.Description)
            .ToList()
        );
    }
}