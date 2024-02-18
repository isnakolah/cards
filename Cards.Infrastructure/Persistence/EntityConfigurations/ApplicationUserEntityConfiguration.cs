using Cards.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cards.Infrastructure.Persistence.EntityConfigurations;

internal sealed class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .HasKey(user => user.Id);
        
        builder
            .Property(user => user.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(user => user.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(user => user.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(user => user.PasswordHash)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(user => user.Role)
            .IsRequired();
    }
}