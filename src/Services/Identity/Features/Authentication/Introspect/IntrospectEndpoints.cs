namespace Identity.API.Features.Authentication.Introspect;
public record IntrospectRequest(string Token);
public record IntrospectResponse(bool Valid);
public class IntrospectEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/introspect", async (IntrospectRequest request, ISender sender) =>
        {
            var command = request.Adapt<IntrospectCommand>();
            var result = await sender.Send(command);
            return Results.Ok(result.Adapt<IntrospectResponse>());
        })
        .MapToApiVersion(1)
        .WithName("Introspect");
    }
}
