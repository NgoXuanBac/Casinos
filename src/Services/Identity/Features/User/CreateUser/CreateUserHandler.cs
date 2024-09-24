using Identity.API.Common;
using Identity.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Features.User.CreateUser;

public record CreateUserCommand(string Email, string Password) : ICommand;

public class CreateUserCommandValidator
    : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email must be a valid email address");
        RuleFor(x => x.Password)
            .Length(6, 20)
            .WithMessage("Password must be between 6 and 20 characters");
    }
}

public class CreateUserHandler(IdentityContext _context)
    : ICommandHandler<CreateUserCommand>
{
    public async Task<Unit> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var hashPassword = PasswordHasher.HashPassword(request.Password);
        var role = await _context.Roles
            .FirstOrDefaultAsync(x => x.Name == "DEFAULT_USER", cancellationToken);
        var user = new Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Password = hashPassword,
            Roles = role != null ? [role] : []
        };
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}