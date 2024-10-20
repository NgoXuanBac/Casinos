using Identity.Application.Features.Permission;

namespace Identity.API.Endpoints
{
    public record CreatePermissionRequest(string Name, string Description, string Url, string Method);
    public class CreatePermissionEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/permission", async (CreatePermissionRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreatePermissionCommand>();
                await sender.Send(command);
                return Results.Created();
            })
            .MapToApiVersion(1)
            .RequirePermission()
            .WithName("CreatePermission");
        }
    }
}