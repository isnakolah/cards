using AutoMapper;
using Cards.Application.Cards.ViewModels;
using Cards.Application.Common.Extensions;
using Cards.Application.Common.Models;
using Cards.Application.Common.Services;
using Cards.Domain.Entities;
using Cards.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cards.Application.Cards.Commands;

public sealed record UpdateCardCommand(
        Guid Id,
        string? Name,
        string? Description,
        string? Color,
        CardStatus? Status) 
    : IRequest<ApiResponse<CardVm>>;
    
public sealed class UpdateCardCommandHandler(
        ICurrentUserService currentUserService,
        IApplicationDbContext dbContext,
        IMapper mapper)
    : IRequestHandler<UpdateCardCommand, ApiResponse<CardVm>>
{
    public async Task<ApiResponse<CardVm>> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
    {
        var userIsAdmin = currentUserService.Role is ApplicationUserRole.Admin;

        var card = await dbContext.Cards
            .ConditionalWhere(
                condition: !userIsAdmin,
                predicate: card => card.User.Id == currentUserService.UserId)
            .FirstOrDefaultAsync(card => card.Id == request.Id, cancellationToken);

        if (card is null)
        {
            return ApiResponse<CardVm>.Error("Card not found");
        }

        try
        {
            card.Update(
                name: request.Name?.Trim(),
                description: request.Description?.Trim(),
                color: request.Color ?? CardColor.DefaultColor!,
                status: request.Status);
        }
        catch (Exception e)
        {
            return ApiResponse<CardVm>.Error(e.Message);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        
        return ApiResponse<CardVm>.Success(mapper.Map<CardVm>(card));
    }
} 