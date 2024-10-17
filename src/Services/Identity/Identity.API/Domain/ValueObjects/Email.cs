using System.Text.RegularExpressions;

namespace Identity.API.Domain.ValueObjects;

public partial record Email
{
    public const int MaxLength = 255;
    public string Value { get; }
    private Email(string value)
    {
        Value = value;
    }

    public static Email Of(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(email.Length, MaxLength);


        if (!EmailRegex().IsMatch(email))
            throw new ArgumentException("Invalid email format.");

        return new Email(email);
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex EmailRegex();
}