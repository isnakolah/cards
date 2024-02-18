namespace Cards.Application.Common.Services;

public interface IDateTimeService
{
    DateTime UtcNow { get; }
}