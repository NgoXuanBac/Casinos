namespace BuildingBlocks.Models.Response;

internal abstract record Response(bool Success);
internal record Failure(object? Error = null) : Response(false);
internal record Success(object? Result = null) : Response(true);
