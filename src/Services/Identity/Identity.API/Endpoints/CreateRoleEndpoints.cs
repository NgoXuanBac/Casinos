using Identity.Application.Features.Role;

namespace Identity.API.Endpoints
{
    public record CreateRoleRequest(string Name, string Description);

    public class CreateRoleEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/role", async (CreateRoleRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateRoleCommand>();
                await sender.Send(command);
                return Results.Created();
            })
            .MapToApiVersion(1)
            .WithName("CreateRole");
        }
    }
}