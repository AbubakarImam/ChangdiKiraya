namespace SpaceRent.Domain.Entities;

public class Amenity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty; // e.g. "wifi", "parking", "ac" — for frontend icon mapping
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Property
    public ICollection<SpaceAmenity> SpaceAmenities { get; set; } = new List<SpaceAmenity>();
}
