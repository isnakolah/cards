using System.Security.Claims;
using Cards.Application.Common.Services;
using Cards.Domain.Entities;

namespace Cards.Api.Services;

internal sealed class CurrentUserService(
    IHttpContextAccessor contextAccessor) : ICurrentUserService
{
    public Guid UserId { get; } = 
        Guid.Parse(contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Actor) ?? Guid.Empty.ToString());
    public ApplicationUserRole Role { get; } = 
        Enum.Parse<ApplicationUserRole>(contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role) ?? throw new InvalidOperationException());
}