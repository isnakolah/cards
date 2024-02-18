using Cards.Application.Common.Extensions;
using Cards.Application.Common.Models;
using Cards.Application.Common.Services;
using Cards.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cards.Application.Cards.Commands;

public sealed record DeleteCardCommand(Guid Id) 
    : IRequest<ApiResponse<bool>>;

public sealed class DeleteCardCommandHandler(
        ICurrentUserService currentUser,
        IApplicationDbContext dbContext) 
    : IRequestHandler<DeleteCardCommand, ApiResponse<bool>>
{
    public async Task<ApiResponse<bool>> Handle(DeleteCardCommand request, CancellationToken cancellationToken)
    {
        var userIsAdmin = currentUser.Role is ApplicationUserRole.Admin;
        
        var card = await dbContext.Cards
            .ConditionalWhere(
                condition: !userIsAdmin ,
                predicate: card => card.User.Id == currentUser.UserId)
            .FirstOrDefaultAsync(card => card.Id == request.Id, cancellationToken);

        if (card is null)
        {
            return ApiResponse<bool>.Error("Card not found");
        }
        
        dbContext.Cards.Remove(card);

        await dbContext.SaveChangesAsync(cancellationToken);
        
        return ApiResponse<bool>.Success(true, "Card deleted successfully");
    }
}
