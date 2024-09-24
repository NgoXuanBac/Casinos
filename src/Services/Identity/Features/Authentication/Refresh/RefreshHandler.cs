namespace Identity.API.Features.Authentication.Refresh;
public record RefreshResult(string Token, bool Authenticated);
public record RefreshCommand(string Token) : ICommand<RefreshResult>;
public class RefreshCommandValidator
    : AbstractValidator<RefreshCommand>
{
    public RefreshCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty().WithMessage("Token cannot be empty");
    }
}

public class RefreshHandler
    : ICommandHandler<RefreshCommand, RefreshResult>
{
    public async Task<RefreshResult> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(10, cancellationToken);
        return new RefreshResult("my_token", true);
    }
}