using Cards.Domain.Entities;

namespace Cards.Application.Common.Services;

public interface ICurrentUserService
{
    Guid UserId { get; }
    ApplicationUserRole Role { get; }
}