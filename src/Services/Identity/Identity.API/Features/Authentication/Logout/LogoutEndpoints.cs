namespace Identity.API.Features.Authentication.Logout;
public record LogoutRequest(string Token);
public class LogoutEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/logout", async (LogoutRequest request, ISender sender) =>
        {
            var command = request.Adapt<LogoutCommand>();
            await sender.Send(command);
            return Results.Ok();
        })
        .WithName("Logout");
    }
}
