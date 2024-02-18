using System.ComponentModel.DataAnnotations;

namespace Cards.Infrastructure.Options;

public sealed class JwtOptions
{
    [Required] public string Key { get; init; } = default!;
    [Required] public string Issuer { get; init; } = default!;
    public string Audience { get; init; } = default!;
}