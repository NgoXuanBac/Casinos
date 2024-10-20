using FluentValidation;
using Identity.Application.Interfaces;
using Identity.Application.Interfaces.Security;

namespace Identity.Application.Features.User
{
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

    public class CreateUserHandler(IApplicationDbContext context, IPasswordHasher passwordHasher)
        : ICommandHandler<CreateUserCommand>
    {
        public async Task<Unit> Handle(CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var hashPassword = passwordHasher.HashPassword(request.Password);
            var role = await context.Roles
                .FirstOrDefaultAsync(x => x.Name == RoleName.Of("base.user"), cancellationToken);
            var user = new Domain.Models.User
            {
                Id = Guid.NewGuid(),
                Email = Email.Of(request.Email),
                Password = Password.Of(hashPassword),
                Roles = role != null ? [role] : []
            };
            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}