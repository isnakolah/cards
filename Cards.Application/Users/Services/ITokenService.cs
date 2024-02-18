using Cards.Domain.Entities;

namespace Cards.Application.Users.Services;

public interface ITokenService
{
    (string Token, int ExpiresInMs) GenerateToken(Guid userId, string email, ApplicationUserRole role);
}