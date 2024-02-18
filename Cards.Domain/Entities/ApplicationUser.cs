using Cards.Domain.Entities.Common;
using Cards.Domain.ValueObjects;

namespace Cards.Domain.Entities;

public sealed record ApplicationUser : BaseEntity
{
    private readonly List<Card> _cards = new();
    private ApplicationUser()
    {
    }

    public string FirstName { get; private init; } = string.Empty;
    public string LastName { get; private init; } = string.Empty;
    public string Email { get; private init; } = string.Empty;
    public string PasswordHash { get; private init; } = string.Empty;
    public ApplicationUserRole Role { get; private init; }
    
    public IReadOnlyCollection<Card> Cards => _cards.AsReadOnly();

    public static ApplicationUser Create(string firstName, string lastName, string email, ApplicationUserRole role, string PasswordHash)
    {
        return new ApplicationUser
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Role = role,
            PasswordHash = PasswordHash
        };
    }

    public void AddCard(string name, string? description, CardColor? color)
    {
        _cards.Add(Card.Create(this, name, description, color));
    }
}
