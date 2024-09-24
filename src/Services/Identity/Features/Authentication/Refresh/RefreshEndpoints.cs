namespace Identity.API.Features.Authentication.Refresh;
public record RefreshRequest(string Token);
public record RefreshResponse(string Token, bool Authenticated);
public class RefreshEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/refresh", async (RefreshRequest request, ISender sender) =>
        {
            var command = request.Adapt<RefreshCommand>();
            var result = await sender.Send(command);
            return Results.Ok(result.Adapt<RefreshResponse>());
        })
        .MapToApiVersion(1)
        .WithName("Refresh");
    }
}
