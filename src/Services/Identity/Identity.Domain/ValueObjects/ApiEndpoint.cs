namespace Identity.Domain.ValueObjects;

public record ApiEndpoint
{
    public const int MAX_PATH_LENGTH = 255;
    public string Path { get; }
    public string Method { get; }

    private ApiEndpoint(string path, string method)
    {
        Path = path;
        Method = method;
    }

    public static ApiEndpoint Of(string path, Method method)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(Path));
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(path.Length, MAX_PATH_LENGTH, nameof(Path));
        return new ApiEndpoint(path, method.ToString());
    }
}