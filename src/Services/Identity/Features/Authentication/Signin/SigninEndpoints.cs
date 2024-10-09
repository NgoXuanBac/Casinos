namespace Identity.Features.Authentication.Signin;
public record SigninRequest(string Email, string Password);
public record SigninResponse(string AccessToken);
public class SigninEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/signin", async (SigninRequest request, ISender sender) =>
        {
            var command = request.Adapt<SigninCommand>();
            var result = await sender.Send(command);
            return Results.Ok(result.Adapt<SigninResponse>());
        })
        .MapToApiVersion(1)
        .WithName("Signin");
    }
}
