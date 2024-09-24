namespace Identity.Features.User.CreateUser;

public record CreateUserRequest(string Email, string Password);
public class CreateUserEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/user", async (CreateUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateUserCommand>();
            var result = await sender.Send(command);
            return Results.Created();
        })
        .MapToApiVersion(1)
        .WithName("CreateUser");
    }
}
