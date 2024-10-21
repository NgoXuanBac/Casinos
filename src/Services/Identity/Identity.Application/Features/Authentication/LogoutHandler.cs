using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;

namespace Identity.Application.Features.Authentication
{
    public record LogoutCommand(string TokenId, string TokenExp) : ICommand;
    public class LogoutCommandValidator
        : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(x => x.TokenId).NotEmpty().WithMessage("Not found token id");
            RuleFor(x => x.TokenExp).NotEmpty().WithMessage("Not found token exp");
        }
    }

    public class LogoutHandler(
        IDistributedCache cache)
        : ICommandHandler<LogoutCommand>
    {
        public async Task<Unit> Handle(LogoutCommand request,
            CancellationToken cancellationToken)
        {

            if (long.TryParse(request.TokenExp, out long ticks))
            {
                await cache.SetStringAsync($"blacklist:{request.TokenId}", "1", new()
                {
                    AbsoluteExpiration = DateTimeOffset.FromUnixTimeSeconds(ticks).UtcDateTime
                }, cancellationToken);
            }
            return Unit.Value;
        }
    }
}
