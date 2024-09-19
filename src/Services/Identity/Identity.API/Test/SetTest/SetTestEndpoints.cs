namespace Identity.API.Test.SetTest;
public record SetTestRequest(string Name);
public class SetTestEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/identity/test", async (SetTestRequest request, ISender sender) =>
        {
            var command = request.Adapt<SetTestCommand>();
            await sender.Send(command);
            return Results.Ok("Set successfully");
        })
        .WithName("SetTest");
    }
}
