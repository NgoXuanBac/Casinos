namespace Identity.Features.Authentication.Logout;
public record LogoutCommand(string Token) : ICommand;
public class LogoutCommandValidator
    : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty().WithMessage("Token cannot be empty");
    }
}

public class LogoutHandler
    : ICommandHandler<LogoutCommand>
{
    public async Task<Unit> Handle(LogoutCommand request,
        CancellationToken cancellationToken)
    {
        await Task.Delay(10, cancellationToken);
        return Unit.Value;
    }
}