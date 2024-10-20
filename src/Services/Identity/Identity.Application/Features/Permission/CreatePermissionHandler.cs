using FluentValidation;
using Identity.Application.Interfaces;
using Identity.Domain.Enums;

namespace Identity.Application.Features.Permission
{
    public record CreatePermissionCommand(string Name, string Description, string Url, string Method) : ICommand;
    public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
    {
        public CreatePermissionCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description cannot be empty");
            RuleFor(x => x.Url).NotEmpty().WithMessage("Url cannot be empty");
            RuleFor(x => x.Method).NotEmpty().WithMessage("Method cannot be empty");
        }
    }

    public class CreatePermissionHandler(IApplicationDbContext context)
        : ICommandHandler<CreatePermissionCommand>
    {
        public async Task<Unit> Handle(CreatePermissionCommand request,
            CancellationToken cancellationToken)
        {
            var permission = new Domain.Models.Permission
            {
                Name = PermissionName.Of(request.Name),
                Description = Description.Of(request.Description),
                Endpoint = ApiEndpoint.Of(request.Url, Method.Get)
            };
            await context.Permissions.AddAsync(permission, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}