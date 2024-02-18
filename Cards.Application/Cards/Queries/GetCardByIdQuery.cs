using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cards.Application.Cards.ViewModels;
using Cards.Application.Common.Extensions;
using Cards.Application.Common.Models;
using Cards.Application.Common.Services;
using Cards.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cards.Application.Cards.Queries;

public sealed record GetCardByIdQuery(Guid Id)
    : IRequest<ApiResponse<CardVm>>;

public sealed class GetCardByIdQueryHandler(
        IApplicationDbContext dbContext,
        IConfigurationProvider mapperCfg,
        ICurrentUserService currentUserService)
    : IRequestHandler<GetCardByIdQuery, ApiResponse<CardVm>>
{
    public async Task<ApiResponse<CardVm>> Handle(GetCardByIdQuery request, CancellationToken cancellationToken)
    {
        var userIsAdmin = currentUserService.Role is ApplicationUserRole.Admin;
        
        var cardVm = await dbContext.Cards
            .ConditionalWhere(
                condition: !userIsAdmin,
                predicate: card => card.User.Id == currentUserService.UserId)
            .ProjectTo<CardVm>(mapperCfg)
            .FirstOrDefaultAsync(card => card.Id == request.Id, cancellationToken);
        
        return cardVm is not null
            ? ApiResponse<CardVm>.Success(cardVm)
            : ApiResponse<CardVm>.Error("Card not found");
    }
}