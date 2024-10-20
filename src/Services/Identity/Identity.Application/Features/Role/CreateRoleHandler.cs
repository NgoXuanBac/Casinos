using FluentValidation;
using Identity.Application.Interfaces;

namespace Identity.Application.Features.Role
{
    public record CreateRoleCommand(string Name, string Description) : ICommand;

    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description cannot be empty");
        }
    }

    public class CreateRoleHandler(IApplicationDbContext context)
        : ICommandHandler<CreateRoleCommand>
    {
        public async Task<Unit> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = new Domain.Models.Role
            {
                Name = RoleName.Of(request.Name),
                Description = Description.Of(request.Description),
            };
            await context.Roles.AddAsync(role, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}