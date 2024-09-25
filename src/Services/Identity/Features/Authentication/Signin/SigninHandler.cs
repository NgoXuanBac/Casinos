using Identity.Common;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Identity.Features.Authentication.Signin;
public record SigninResult(string Token, bool Authenticated);
public record SigninCommand(string Email, string Password) : ICommand<SigninResult>;
public class SigninCommandValidator
    : AbstractValidator<SigninCommand>
{
    public SigninCommandValidator()
    {
        RuleFor(x => x.Email).Length(6, 20)
            .EmailAddress()
            .WithMessage("Email must be a valid email address");
        RuleFor(x => x.Password).Length(6, 20)
            .WithMessage("Password must be between 6 and 20 characters");
    }
}

public class SigninHandler(TokenService _tokenService, IdentityContext _context)
    : ICommandHandler<SigninCommand, SigninResult>
{
    public async Task<SigninResult> Handle(SigninCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.Roles).ThenInclude(r => r.Permissions)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken)
                ?? throw new Exception("User not found");

        if (!PasswordHasher.VerifyPassword(request.Password, user.Password))
            throw new Exception($"Invalid password");
        var token = _tokenService.GenerateToken(user);
        return new SigninResult(token, true);
    }
}