using System.Drawing;
using Cards.Domain.Entities.Common;
using Cards.Domain.ValueObjects;

namespace Cards.Domain.Entities;

public sealed record Card : BaseEntity
{
    private Card()
    {
    }
    
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public CardColor? Color { get; private set; }
    public CardStatus Status { get; private set; }
    public ApplicationUser User { get; private init; } = default!;

    internal static Card Create(ApplicationUser user, string name, string? description, CardColor? color)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Card name cannot be empty or whitespace.", nameof(name));

        return new Card
        {
            User = user,
            Name = name.Trim(),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            Color = color,
            Status = CardStatus.ToDo
        };
    }
    
    public void Update(string? name = null, string? description = null, CardColor? color = null, CardStatus? status = null)
    {
        Name = name ?? Name;
        Status = status ?? Status;
        Color = color;
        Description = description;
    }

    public static Color? DefaultColor => null;
}