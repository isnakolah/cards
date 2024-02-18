using Cards.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cards.Infrastructure.Persistence.EntityConfigurations;

internal sealed class CardEntityConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder
            .ToTable(table => table.IsTemporal());

        builder
            .Property(card => card.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder
            .Property(card => card.Description)
            .HasMaxLength(500);
        
        builder
            .Property(propertyExpression: card => card.Status)
            .HasConversion(
                convertToProviderExpression: status => status.ToString(),
                convertFromProviderExpression: status => Enum.Parse<CardStatus>(status));
        
        builder
            .Property(propertyExpression: card => card.Color)
            .HasConversion(
                convertToProviderExpression: color => !string.IsNullOrWhiteSpace(color!) ? color.ToString() : null!,
                convertFromProviderExpression: color => !string.IsNullOrWhiteSpace(color) ? color : null!);
    }
}