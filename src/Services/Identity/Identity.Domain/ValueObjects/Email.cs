using System.Text.RegularExpressions;

namespace Identity.Domain.ValueObjects;

public partial record Email
{
    public const int MAX_LENGTH = 255;
    public string Value { get; }
    private Email(string value)
    {
        Value = value;
    }

    public static Email Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(Email));
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value.Length, MAX_LENGTH, nameof(Email));


        if (!EmailRegex().IsMatch(value))
            throw new ArgumentException("Invalid email format.");

        return new Email(value);
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex EmailRegex();
}