namespace Cards.Domain.Entities.Common;

public abstract record BaseEntity
{
    public Guid Id { get; init; }
    
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    
    public DateTime? DateDeleted { get; set; }
    
    public bool IsDeleted { get; set; }
}