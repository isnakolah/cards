using Cards.Application.Common.Services;

namespace Cards.Infrastructure.Services;

internal sealed class UtcNowService : IDateTimeService
{
    public DateTime UtcNow { get; } = DateTime.UtcNow;
}