using AutoMapper;
using Cards.Application.Common.Mapping;
using Cards.Domain.Entities;

namespace Cards.Application.Users.ViewModels;

public sealed record UserVm : IViewModel<ApplicationUser>
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;

    public static void Mapping(Profile profile)
    {
        profile
            .CreateMap<ApplicationUser, UserVm>()
            .ForMember(vm => vm.FullName, opt => opt.MapFrom(user => $"{user.FirstName} {user.LastName}"));
    }
} 