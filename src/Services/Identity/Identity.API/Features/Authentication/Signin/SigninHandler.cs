namespace Identity.API.Features.Authentication.Signin;
public record SigninResult(string Token, bool Authenticated);
public record SigninCommand(string Username, string Password) : ICommand<SigninResult>;
public class SigninCommandValidator
    : AbstractValidator<SigninCommand>
{
    public SigninCommandValidator()
    {
        RuleFor(x => x.Username).Length(6, 20)
            .WithMessage("Username must be between 6 and 20 characters");
        RuleFor(x => x.Password).Length(6, 20)
            .WithMessage("Password must be between 6 and 20 characters");
    }
}

public class SigninHandler
    : ICommandHandler<SigninCommand, SigninResult>
{
    public async Task<SigninResult> Handle(SigninCommand request,
        CancellationToken cancellationToken)
    {
        await Task.Delay(10, cancellationToken);
        return new SigninResult("my_token", true);
    }
}