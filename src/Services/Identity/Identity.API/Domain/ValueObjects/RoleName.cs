namespace Identity.API.Domain.ValueObjects;

public record RoleName
{
    public const int MaxLength = 255;

    public string Value { get; }
    private RoleName(string value) => Value = value;

    public static RoleName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value.Length, MaxLength);
        return new RoleName(value);
    }
}