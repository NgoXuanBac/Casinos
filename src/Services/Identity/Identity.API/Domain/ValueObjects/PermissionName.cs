namespace Identity.API.Domain.ValueObjects;

public record PermissionName
{
    public const int MaxLength = 255;

    public string Value { get; }
    private PermissionName(string value) => Value = value;

    public static PermissionName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value.Length, MaxLength);
        return new PermissionName(value);
    }
}