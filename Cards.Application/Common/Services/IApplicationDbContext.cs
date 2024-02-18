using Cards.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cards.Application.Common.Services;

public interface IApplicationDbContext
{
    DbSet<Card> Cards { get; }
    DbSet<ApplicationUser> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}