using System.Security.Claims;

namespace Nummi.Core.Bridge; 

public interface IJwtMinter {
    Jwt MintToken(IList<Claim> claims);
}