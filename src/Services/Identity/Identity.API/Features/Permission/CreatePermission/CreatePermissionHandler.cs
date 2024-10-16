using Identity.API.Infrastructure.Data;

namespace Identity.API.Features.Permission.CreatePermission
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

    public class CreatePermissionHandler(IdentityContext context)
        : ICommandHandler<CreatePermissionCommand>
    {
        public async Task<Unit> Handle(CreatePermissionCommand request,
            CancellationToken cancellationToken)
        {
            var permission = new Domain.Models.Permission
            {
                Name = request.Name,
                Description = request.Description,
                Path = request.Url,
                Method = request.Method
            };
            await context.Permissions.AddAsync(permission, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}