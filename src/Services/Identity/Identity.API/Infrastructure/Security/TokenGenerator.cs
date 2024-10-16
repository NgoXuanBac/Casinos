using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.Infrastructure.Security
{
    public record TokenConfig(string SigningKey,
        double AccessTokenExp);
    public class TokenGenerator
    {
        private readonly SymmetricSecurityKey _key;
        private readonly TokenConfig _config;
        public TokenGenerator(IConfiguration configuration)
        {
            _config = configuration.GetSection(nameof(TokenConfig)).Get<TokenConfig>()
                ?? throw new Exception($"{nameof(TokenConfig)} isn't found");

            _key = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_config.SigningKey));
        }

        public TokenConfig GetTokenConfig() => _config;

        public string GenerateJWT(IEnumerable<Claim> claims)
        {
            var exp = _config.AccessTokenExp;
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new(claims),
                Expires = DateTime.Now.AddMinutes(exp),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal VerifyJWT(string token, bool expired = false)
        {
            var parameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = !expired,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException();
            return principal;
        }
    }
}