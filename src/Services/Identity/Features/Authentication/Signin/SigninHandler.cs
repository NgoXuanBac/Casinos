using System.Security.Claims;
using System.Text.Json;
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
        var hashPassword = PasswordHasher.HashPassword(request.Password);
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken)
                ?? throw new Exception("User not found");

        if (!PasswordHasher.VerifyPassword(user.Password, hashPassword))
            throw new Exception("Invalid password");

        var permissionsClaim = await _context.Permissions
            .AsNoTracking()
            .Where(p => p.Url == request.Email)
            .ToListAsync(cancellationToken);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new("Permissions", JsonSerializer.Serialize(permissionsClaim))
        };

        var token = _tokenService.GenerateToken(claims);
        return new SigninResult(token, true);
    }
}