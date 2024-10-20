namespace Identity.Domain.ValueObjects;

public record Description
{
    public const int MAX_LENGTH = 500;

    public string Value { get; }
    private Description(string value) => Value = value;

    public static Description Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value.Length, MAX_LENGTH);
        return new Description(value);
    }
}