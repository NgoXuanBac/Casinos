using Identity.Infrastructure.Data;
using Identity.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace Identity.Features.User.CreateUser;

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

public class CreateUserHandler(IdentityContext context, PasswordHasher passwordHasher)
    : ICommandHandler<CreateUserCommand>
{
    public async Task<Unit> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var hashPassword = passwordHasher.HashPassword(request.Password);
        var role = await context.Roles
            .FirstOrDefaultAsync(x => x.Name == "base.user", cancellationToken);
        var user = new Domain.Models.User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Password = hashPassword,
            Roles = role != null ? [role] : []
        };
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}