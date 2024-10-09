
using Identity.Infrastructure.Data;

namespace Identity.Features.Role.CreateRole;

public record CreateRoleCommand(string Name, string Description) : ICommand;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description cannot be empty");
    }
}

public class CreateRoleHandler(IdentityContext context) : ICommandHandler<CreateRoleCommand>
{
    public async Task<Unit> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new Domain.Models.Role
        {
            Name = request.Name,
            Description = request.Description
        };
        await context.Roles.AddAsync(role, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}