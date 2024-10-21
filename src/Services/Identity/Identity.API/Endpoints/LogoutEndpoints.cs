using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Identity.Application.Features.Authentication;

namespace Identity.API.Endpoints
{
    public class LogoutEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/logout", async (ISender sender, HttpContext context) =>
            {
                var tokenId = context.User.FindFirstValue(JwtRegisteredClaimNames.Jti) ?? string.Empty;
                var tokenExp = context.User.FindFirstValue(JwtRegisteredClaimNames.Exp) ?? string.Empty;
                await sender.Send(new LogoutCommand(tokenId, tokenExp));
                return Results.Ok();
            })
            .RequirePermission()
            .MapToApiVersion(1)
            .WithName("Logout");
        }
    }
}
