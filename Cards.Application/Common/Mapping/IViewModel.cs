using AutoMapper;

namespace Cards.Application.Common.Mapping;

public interface IViewModel<T>
{
    public Guid Id { get; init; }

    public static void Mapping(Profile profile)
    {
        profile.CreateMap<T, IViewModel<T>>();
    }
}