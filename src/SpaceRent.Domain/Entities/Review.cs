namespace SpaceRent.Domain.Entities;

public class Review
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SpaceId { get; set; }
    public Guid UserId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual Space? Space { get; set; }
    public virtual User? User { get; set; }
}
