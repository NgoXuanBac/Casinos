
namespace Identity.API.Test.SetTest;

public record SetTestCommand(string Name, string Message) : ICommand;

public class SetMessageCommandValidator
    : AbstractValidator<SetTestCommand>
{
    public SetMessageCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
        RuleFor(x => x.Message).NotEmpty().WithMessage("Message cannot be empty");
    }
}

public class SetMessageHandler
    : ICommandHandler<SetTestCommand>
{
    public async Task<Unit> Handle(SetTestCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        return Unit.Value;
    }
}