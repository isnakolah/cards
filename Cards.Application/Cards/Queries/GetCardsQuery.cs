using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cards.Application.Cards.Enums;
using Cards.Application.Cards.ViewModels;
using Cards.Application.Common.Extensions;
using Cards.Application.Common.Models;
using Cards.Application.Common.Services;
using Cards.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cards.Application.Cards.Queries;

public sealed record GetCardsQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string? SearchTerm = null,
        CardSortBy? SortBy = null,
        bool DescOrder = false,
        CardStatus? Status = null)
    : IRequest<PaginatedApiResponse<CardVm>>
{
    public int PageNumber { get; } = Math.Max(PageNumber, 1);
    public int PageSize { get; } = Math.Clamp(PageSize, 1, 100);
};

public sealed class GetCardsQueryHandler(
        ICurrentUserService currentUserService,
        IConfigurationProvider mapperCfg,
        IApplicationDbContext dbContext)
    : IRequestHandler<GetCardsQuery, PaginatedApiResponse<CardVm>>
{
    public async Task<PaginatedApiResponse<CardVm>> Handle(GetCardsQuery request, CancellationToken cancellationToken)
    {
        var userIsAdmin = currentUserService.Role is ApplicationUserRole.Admin;

        var query = dbContext.Cards
            .ConditionalWhere(
                condition: !userIsAdmin,
                predicate: card => card.User.Id == currentUserService.UserId)
            .ConditionalWhere(
                condition: request.Status.HasValue,
                predicate: card => card.Status == request.Status)
            .ConditionalWhere(
                condition: !string.IsNullOrWhiteSpace(request.SearchTerm),
                predicate: card => card.Name.Contains(request.SearchTerm!));
        
        query = request.SortBy switch
        {
            CardSortBy.Name => request.DescOrder
                ? query.OrderByDescending(card => card.Name)
                : query.OrderBy(card => card.Name),
            CardSortBy.Status => request.DescOrder
                ? query.OrderByDescending(card => card.Status)
                : query.OrderBy(card => card.Status),
            CardSortBy.Color => request.DescOrder
                ? query.OrderByDescending(card => card.Color)
                : query.OrderBy(card => card.Color),
            CardSortBy.DateCreated => request.DescOrder
                ? query.OrderByDescending(card => card.DateCreated)
                : query.OrderBy(card => card.DateCreated),
            _ => request.DescOrder
                ? query.OrderByDescending(card => card.Id)
                : query.OrderBy(card => card.Id)
        };

        var cardVms = await query
            .Paginate(request.PageNumber, request.PageSize)
            .ProjectTo<CardVm>(mapperCfg)
            .ToArrayAsync(cancellationToken);

        var totalItems = await query.CountAsync(cancellationToken);

        return totalItems > 0
            ? PaginatedApiResponse<CardVm>.Success(cardVms, totalItems, request.PageNumber, request.PageSize)
            : PaginatedApiResponse<CardVm>.Error("No cards found");
    }
}
