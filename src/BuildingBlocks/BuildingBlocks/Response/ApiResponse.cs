namespace BuildingBlocks.Response;

public abstract record ApiResponse(bool Success);
public record ErrorApiResponse(object? Error = null) : ApiResponse(false);
public record SuccessApiResponse(object? Data = null) : ApiResponse(true);
