using SpaceRent.Domain.Enums;

namespace SpaceRent.Domain.Entities;

public class Space
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PricePerHour { get; set; }
    public string Location { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Capacity { get; set; }
    public bool IsPublished { get; set; }
    public Guid OwnerId { get; set; }
    public SpaceType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation Properties
    public User Owner { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<SpaceAmenity> SpaceAmenities { get; set; } = new List<SpaceAmenity>();
    public ICollection<SpaceMedia> Media { get; set; } = new List<SpaceMedia>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
