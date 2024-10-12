using System.Text.Json;
using BuildingBlocks.Auth.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BuildingBlocks.Auth.Middlewares;

record Permission(string Name, string Path, string Method);

public class AuthorizationMiddleware(
    RequestDelegate next)
{
    const string BASE_VERSION_PATH = "/api/v{version:apiVersion}";

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var method = context.Request.Method;
        var path = (endpoint as RouteEndpoint)?.RoutePattern.RawText?
            .Replace(BASE_VERSION_PATH, string.Empty);

        var requiresPermission = endpoint?.Metadata
            .OfType<RequirePermissionAttribute>()
            .Any() == true;

        if (!requiresPermission)
        {
            await next(context);
            return;
        }

        if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }
    
        var permissionsClaim = context.User.Claims
            .FirstOrDefault(x => x.Type == "permissions")?
            .Value;


        var permissions = permissionsClaim != null
            ? JsonSerializer.Deserialize<IEnumerable<Permission>>(permissionsClaim) ?? [] : [];
    
        var hasPermission = permissions.Any(x => string.Equals(path, x.Path, StringComparison.OrdinalIgnoreCase)
                && string.Equals(method, x.Method, StringComparison.OrdinalIgnoreCase));

        if (!hasPermission)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }
        await next(context);
    }
}