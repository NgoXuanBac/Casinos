using System.Security.Claims;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Identity.Features.Authentication.Refresh;
public record RefreshResult(string Token, bool Authenticated);
public record RefreshCommand(string Token) : ICommand<RefreshResult>;
public class RefreshCommandValidator
    : AbstractValidator<RefreshCommand>
{
    public RefreshCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty().WithMessage("Token cannot be empty");
    }
}

public class RefreshHandler(TokenService _tokenService, IdentityContext _context)
    : ICommandHandler<RefreshCommand, RefreshResult>
{
    public async Task<RefreshResult> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var principal = _tokenService.VerifyToken(request.Token, true);
        var email = principal.FindFirstValue(ClaimTypes.Email)
            ?? throw new Exception("Email not found");

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken)
                ?? throw new Exception("User not found");
        var token = _tokenService.GenerateToken(user);
        return new RefreshResult(token, true);
    }
}