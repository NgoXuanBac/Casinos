using Identity.Infrastructure.Services;

namespace Identity.Features.Authentication.Introspect;

public record IntrospectResult(bool Valid);
public record IntrospectCommand(string Token) : ICommand<IntrospectResult>;

public class IntrospectCommandValidator
    : AbstractValidator<IntrospectCommand>
{
    public IntrospectCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty().WithMessage("Token cannot be empty");
    }
}

public class IntrospectHandler(TokenService _tokenService)
    : ICommandHandler<IntrospectCommand, IntrospectResult>
{
    public async Task<IntrospectResult> Handle(IntrospectCommand request,
        CancellationToken cancellationToken)
    {
        var token = request.Token;
        bool valid = true;
        try
        {
            _tokenService.VerifyToken(token);
        }
        catch (Exception)
        {
            valid = false;
        }
        await Task.CompletedTask;
        return new IntrospectResult(valid);
    }


}