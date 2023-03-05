using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Nummi.Core.Bridge.Identity; 

public class JwtMinter : IJwtMinter {
    
    private IConfiguration Configuration { get; }

    public JwtMinter(IConfiguration configuration) {
        Configuration = configuration;
    }
    
    public Jwt MintToken(IList<Claim> authClaims) {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]!));
        var token = new JwtSecurityToken(
            issuer: Configuration["JWT:Issuer"],
            audience: Configuration["JWT:Audience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        var tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
        return new Jwt(tokenAsString);
    }
    
}