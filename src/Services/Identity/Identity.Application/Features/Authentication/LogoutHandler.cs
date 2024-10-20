using System.IdentityModel.Tokens.Jwt;
using FluentValidation;
using Identity.Application.Interfaces.Security;
using Microsoft.Extensions.Caching.Distributed;

namespace Identity.Application.Features.Authentication
{
    public record LogoutCommand(string Token) : ICommand;
    public class LogoutCommandValidator
        : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(x => x.Token).NotEmpty().WithMessage("Token cannot be empty");
        }
    }

    public class LogoutHandler(
        ITokenGenerator tokenGenerator,
        IDistributedCache cache)
        : ICommandHandler<LogoutCommand>
    {
        public async Task<Unit> Handle(LogoutCommand request,
            CancellationToken cancellationToken)
        {
            var principal = tokenGenerator.VerifyJWT(request.Token, true);
            var exp = principal.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)?
                .Value;
            var tokenId = principal.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?
                .Value;
            if (long.TryParse(exp, out long ticks))
            {
                await cache.SetStringAsync($"blacklist:{tokenId}", "1", new()
                {
                    AbsoluteExpiration = DateTimeOffset.FromUnixTimeSeconds(ticks).UtcDateTime
                }, cancellationToken);
            }
            return Unit.Value;
        }
    }
}
