using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Identity.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure.Services;

public record TokenConfig(string SigningKey, double TokenValidityInMinutes,
    double RefreshTokenValidityInDays);
public class TokenService
{
    private readonly SymmetricSecurityKey _key;
    private readonly TokenConfig _config;
    public TokenService(IConfiguration configuration)
    {
        _config = configuration.GetSection(nameof(TokenConfig)).Get<TokenConfig>()
            ?? throw new Exception($"{nameof(TokenConfig)} isn't found");

        _key = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(_config.SigningKey));
    }
    public string GenerateToken(User user)
    {
        var permissions = user.Roles
            .SelectMany(r => r.Permissions)
            .Select(p => new { p.Name, p.Url, p.Method });
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new("permissions", JsonSerializer.Serialize(permissions))
        };
        return GenerateToken(claims);
    }
    public string GenerateToken(List<Claim> claims)
    {
        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new(claims),
            Expires = DateTime.Now.AddMinutes(_config.TokenValidityInMinutes),
            SigningCredentials = credentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal VerifyToken(string token, bool expired = false)
    {
        var parameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = !expired,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _key
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException();
        return principal;
    }
}