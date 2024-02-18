using AutoMapper;
using Cards.Application.Common.Models;
using Cards.Application.Common.Services;
using Cards.Application.Users.Services;
using Cards.Application.Users.ViewModels;
using Cards.Domain.Entities;
using MediatR;

namespace Cards.Application.Users.Commands;

public sealed record SeedUsersCommand
    : IRequest<ApiResponse<UserVm[]>>;
    
public sealed class SeedUsersCommandHandler(
        IHashingService hashingService,
        IApplicationDbContext dbContext,
        IMapper mapper)
    : IRequestHandler<SeedUsersCommand, ApiResponse<UserVm[]>>
{
    public async Task<ApiResponse<UserVm[]>> Handle(SeedUsersCommand request, CancellationToken cancellationToken)
    {
        var users = new[]
        {
            ApplicationUser.Create(
                firstName: "Dante",
                lastName: "Obadiah",
                email: "admin@cards.com",
                role: ApplicationUserRole.Admin,
                PasswordHash: hashingService.Hash("StrongAdminPassword123")),
            ApplicationUser.Create(
                firstName: "John",
                lastName: "Doe",
                email: "john.doe@cards.com",
                role: ApplicationUserRole.Member,
                PasswordHash: hashingService.Hash("StrongMemberOnePassword123")),
            ApplicationUser.Create(
                firstName: "Jane",
                lastName: "Doe",
                email: "jane.doe@cards.com", 
                role: ApplicationUserRole.Member,
                PasswordHash: hashingService.Hash("StrongMemberTwoPassword123")),
        };

        await dbContext.Users.AddRangeAsync(users, cancellationToken);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return ApiResponse<UserVm[]>.Success(mapper.Map<UserVm[]>(users));
    }
}