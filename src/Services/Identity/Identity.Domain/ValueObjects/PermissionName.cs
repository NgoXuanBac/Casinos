namespace Identity.Domain.ValueObjects;

public record PermissionName
{
    public const int MAX_LENGTH = 255;

    public string Value { get; }
    private PermissionName(string value) => Value = value;

    public static PermissionName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value.Length, MAX_LENGTH);
        return new PermissionName(value);
    }
}