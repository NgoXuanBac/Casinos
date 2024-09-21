using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Models.Response.Filter;

public class CustomResponseFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var result = await next(context);
        if (result is Response) return result;
        return new Success(result);
    }
}