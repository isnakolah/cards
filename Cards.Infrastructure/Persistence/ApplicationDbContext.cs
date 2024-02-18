using System.Reflection;
using Cards.Application.Common.Services;
using Cards.Domain.Entities;
using Cards.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Cards.Infrastructure.Persistence;

internal sealed class ApplicationDbContext(
        IDateTimeService dateTimeService,
        DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<Card> Cards => Set<Card>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.DateCreated = dateTimeService.UtcNow;
                    break;
                case EntityState.Deleted:
                    entry.Entity.DateDeleted = dateTimeService.UtcNow;
                    entry.Entity.IsDeleted = true;
                    entry.State = EntityState.Modified;
                    continue;
                case EntityState.Modified:
                    entry.Entity.DateUpdated = dateTimeService.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            var method = SetGlobalQueryForIsDeletedMethod.MakeGenericMethod(entityType.ClrType);

            method.Invoke(this, [modelBuilder]);
        }
    }
    
    private static readonly MethodInfo SetGlobalQueryForIsDeletedMethod = typeof(ApplicationDbContext)
        .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
        .Single(t => t is {IsGenericMethod: true, Name: "SetGlobalQueryForIsDeleted"});

    private void SetGlobalQueryForIsDeleted<T>(ModelBuilder builder) where T : BaseEntity
    {
        builder.Entity<T>().HasQueryFilter(entity => !EF.Property<bool>(entity, "IsDeleted"));
    }
}