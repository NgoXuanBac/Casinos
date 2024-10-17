using Identity.API.Domain.Enums;

namespace Identity.API.Domain.ValueObjects;

public record ApiEndpoint
{
    public const int MaxPathLength = 100;
    public string Path { get; }
    public string Method { get; }

    private ApiEndpoint(string path, string method)
    {
        Path = path;
        Method = method;
    }

    public static ApiEndpoint Of(string path, Method method)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(path);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(path.Length, MaxPathLength);
        return new ApiEndpoint(path, method.ToString());
    }
}