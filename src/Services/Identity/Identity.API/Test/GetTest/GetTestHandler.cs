namespace Identity.API.Test.GetTest;

public record GetTestResult(string Message);
public record GetTestQuery(string Name) : IQuery<GetTestResult>;
public class GetTestHandler :
    IQueryHandler<GetTestQuery, GetTestResult>
{
    public async Task<GetTestResult> Handle(GetTestQuery request,
        CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        return new GetTestResult($"Hello, {request.Name}");
    }
}