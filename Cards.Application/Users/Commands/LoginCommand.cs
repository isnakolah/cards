using AutoMapper;
using Cards.Application.Common.Models;
using Cards.Application.Common.Services;
using Cards.Application.Users.Services;
using Cards.Application.Users.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cards.Application.Users.Commands;

public sealed record LoginCommand(
        string Email,
        string Password) 
    : IRequest<ApiResponse<AccessTokenVm>>;

public sealed class LoginCommandHandler(
        IDateTimeService dateTimeService,
        IApplicationDbContext dbContext,
        IHashingService hashingService,
        ITokenService tokenService,
        IMapper mapper)
    : IRequestHandler<LoginCommand, ApiResponse<AccessTokenVm>>
{
    public async Task<ApiResponse<AccessTokenVm>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(user => user.Email == request.Email, cancellationToken);
        
        if (user is null || !hashingService.Verify(request.Password, user.PasswordHash))
        {
            return ApiResponse<AccessTokenVm>.Error("Invalid email or password");
        }

        var (token, expiresMs) = tokenService.GenerateToken(user.Id, user.Email, user.Role);
        
        return ApiResponse<AccessTokenVm>
            .Success(AccessTokenVm.Create(
                token: token,
                expires: dateTimeService.UtcNow.AddMilliseconds(expiresMs),
                user: mapper.Map<UserVm>(user)));
    }
}
    
    