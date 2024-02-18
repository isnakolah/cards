using AutoMapper;
using Cards.Application.Cards.ViewModels;
using Cards.Application.Common.Models;
using Cards.Application.Common.Services;
using Cards.Domain.ValueObjects;
using MediatR;

namespace Cards.Application.Cards.Commands;

public sealed record CreateCardCommand(
        string Name,
        string? Description,
        string? Color) 
    : IRequest<ApiResponse<CardVm>>;

public sealed class CreateCardCommandHandler(
        ICurrentUserService currentUserService,
        IApplicationDbContext dbContext,
        IMapper mapper)
    : IRequestHandler<CreateCardCommand, ApiResponse<CardVm>>
{
    public async Task<ApiResponse<CardVm>> Handle(CreateCardCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FindAsync([currentUserService.UserId], cancellationToken);
        
        if (user is null)
        {
            return ApiResponse<CardVm>.Error("User not found");
        }

        try
        {
            user.AddCard(
                name: request.Name,
                description: request.Description,
                color: request.Color ?? CardColor.DefaultColor!);
        }
        catch (Exception e)
        {
            return ApiResponse<CardVm>.Error(e.Message);
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return ApiResponse<CardVm>.Success(mapper.Map<CardVm>(user.Cards.Last()));
    }
} 