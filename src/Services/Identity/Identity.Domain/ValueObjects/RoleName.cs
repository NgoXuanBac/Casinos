namespace Identity.Domain.ValueObjects;

public record RoleName
{
    public const int MAX_LENGTH = 255;

    public string Value { get; }
    private RoleName(string value) => Value = value;

    public static RoleName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(RoleName));
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value.Length, MAX_LENGTH, nameof(RoleName));
        return new RoleName(value);
    }


}