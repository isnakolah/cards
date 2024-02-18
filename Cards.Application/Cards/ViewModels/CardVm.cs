using AutoMapper;
using Cards.Application.Common.Mapping;
using Cards.Domain.Entities;

namespace Cards.Application.Cards.ViewModels;

public sealed record CardVm : IViewModel<Card>
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Color { get; init; }
    public required string Status { get; init; }
    public required string User { get; init; }
    public required string UserId { get; init; }

    public static void Mapping(Profile profile)
    {
        profile.CreateMap<Card, CardVm>()
            .ForMember(vm => vm.Status, opt => opt.MapFrom(card => card.Status.ToString()))
            .ForMember(vm => vm.User, opt => opt.MapFrom(card => $"{card.User.FirstName} {card.User.LastName}"))
            .ForMember(vm => vm.UserId, opt => opt.MapFrom(card => card.User.Id));
    }
}