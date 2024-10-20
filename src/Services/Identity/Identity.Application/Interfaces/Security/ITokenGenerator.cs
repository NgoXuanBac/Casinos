using System.Security.Claims;

namespace Identity.Application.Interfaces.Security;

public record TokenConfig(string SigningKey,
    double AccessTokenExp);

public interface ITokenGenerator
{
    string GenerateJWT(IEnumerable<Claim> claims);
    ClaimsPrincipal VerifyJWT(string token, bool expired = false);
    TokenConfig GetTokenConfig();
}