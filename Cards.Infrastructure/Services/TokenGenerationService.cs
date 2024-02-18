using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cards.Application.Users.Services;
using Cards.Domain.Entities;
using Cards.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Cards.Infrastructure.Services;

internal sealed class TokenGenerationService(
        IOptions<JwtOptions> _jwtOptions)
    : ITokenService
{
    public (string Token, int ExpiresInMs) GenerateToken(Guid userId, string email, ApplicationUserRole role)
    {
        var claims = new List<Claim>
        {
            new ( ClaimTypes.Actor, userId.ToString()),
            new (ClaimTypes.Email, email),
            new(ClaimTypes.Role, role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.Key));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var expiresIn = TimeSpan.FromDays(1);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            issuer: _jwtOptions.Value.Issuer,
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return (tokenString, expiresIn.Milliseconds);
    }
}