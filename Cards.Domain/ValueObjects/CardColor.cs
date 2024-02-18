using System.Text.RegularExpressions;

namespace Cards.Domain.ValueObjects;

public sealed partial record CardColor
{
    private readonly string value;

    private CardColor(string color)
    {
        if (string.IsNullOrWhiteSpace(color))
            throw new ArgumentException("Color cannot be empty or whitespace.", nameof(color));

        if (!ColorPatternRegex().IsMatch(color))
            throw new ArgumentException("Color must be a # followed by 6 alphanumeric characters.", nameof(color));

        value = color;
    }

    public static implicit operator CardColor?(string? color)
    {
        return string.IsNullOrWhiteSpace(color)
            ? DefaultColor
            : new CardColor(color.Trim());
    }

    public static implicit operator string(CardColor cardColor)
    {
        return cardColor.value;
    }

    public override string ToString()
    {
        return value;
    }

    [GeneratedRegex(@"^#[A-Za-z0-9]{6}$")]
    private static partial Regex ColorPatternRegex();

    private static CardColor? DefaultColor => null;
}