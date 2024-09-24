using Identity.Infrastructure.Data;

namespace Identity.Features.Permission.CreatePermission;
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

public class CreatePermissionHandler(IdentityContext _context)
    : ICommandHandler<CreatePermissionCommand>
{
    public async Task<Unit> Handle(CreatePermissionCommand request,
        CancellationToken cancellationToken)
    {
        var permission = new Domain.Entities.Permission
        {
            Name = request.Name,
            Description = request.Description,
            Url = request.Url,
            Method = request.Method
        };
        await _context.Permissions.AddAsync(permission, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}