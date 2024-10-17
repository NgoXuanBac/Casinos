namespace Identity.API.Domain.ValueObjects;

public record Description
{
    public const int MaxLength = 500;

    public string Value { get; }
    private Description(string value) => Value = value;

    public static Description Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value.Length, MaxLength);
        return new Description(value);
    }
}