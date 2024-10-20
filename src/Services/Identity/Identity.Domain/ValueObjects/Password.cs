namespace Identity.Domain.ValueObjects;

public record Password
{
    public const int MAX_LENGTH = 255;
    public string Value { get; }
    private Password(string value)
    {
        Value = value;
    }
    public static Password Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(Password));
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value.Length, MAX_LENGTH, nameof(Password));
        return new Password(value);
    }
}