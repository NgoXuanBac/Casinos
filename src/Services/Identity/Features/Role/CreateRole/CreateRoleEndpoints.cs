
namespace Identity.Features.Role.CreateRole;

public record CreateRoleRequest(string Name, string Description);

public class CreateRoleEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/role", async (CreateRoleRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateRoleCommand>();
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .MapToApiVersion(1)
        .WithName("CreateRole");
    }
}