namespace SpaceRent.Domain.Entities;

/// <summary>
/// Join entity for the many-to-many relationship between Space and Amenity.
/// </summary>
public class SpaceAmenity
{
    public Guid SpaceId { get; set; }
    public Space Space { get; set; } = null!;

    public Guid AmenityId { get; set; }
    public Amenity Amenity { get; set; } = null!;
}
