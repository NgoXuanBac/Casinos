using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Response.Filters;

public class StandardResponseFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var result = await next(context);
        if (result is ApiResponse) return result;
        return new SuccessApiResponse(result);
    }
}