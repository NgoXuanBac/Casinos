using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Test.GetTest;
public record GetTestRequest(string Name);
public record GetTestResponse(string Message);
public class GetTestEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/identity/test", async ([FromBody] GetTestRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetTestQuery>();
            var result = await sender.Send(query);
            return Results.Ok(result.Message);
        })
        .WithName("GetTest");
    }
}
