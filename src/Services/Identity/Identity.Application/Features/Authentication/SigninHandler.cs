using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using FluentValidation;
using Identity.Application.Interfaces;
using Identity.Application.Interfaces.Security;

namespace Identity.Application.Features.Authentication
{
    public record SigninResult(string AccessToken);
    public record SigninCommand(string Email, string Password)
        : ICommand<SigninResult>;
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

    public class SigninHandler(IApplicationDbContext context,
        ITokenGenerator tokenGenerator,
        IPasswordHasher passwordHasher)
        : ICommandHandler<SigninCommand, SigninResult>
    {
        public async Task<SigninResult> Handle(SigninCommand request,
            CancellationToken cancellationToken)
        {
            var user = await context.Users
                .Include(u => u.Roles).ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Email == Email.Of(request.Email), cancellationToken)
                    ?? throw new Exception("Email is not found");

            if (!passwordHasher.VerifyPassword(request.Password, user.Password.Value))
                throw new Exception("Password is incorrect");

            var permissions = user.Roles
                .SelectMany(r => r.Permissions)
                .Select(p => new { p.Name.Value, p.Endpoint.Path, p.Endpoint.Method });

            var tokenId = Guid.NewGuid().ToString();

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, tokenId),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email.Value),
                new("permissions", JsonSerializer.Serialize(permissions))
            };

            var accessToken = tokenGenerator.GenerateJWT(claims);
            await context.SaveChangesAsync(cancellationToken);

            return new SigninResult(accessToken);
        }
    }
}