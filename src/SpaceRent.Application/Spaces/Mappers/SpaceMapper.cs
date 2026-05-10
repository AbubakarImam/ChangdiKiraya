using SpaceRent.Application.Amenities.DTOs;
using SpaceRent.Application.Spaces.DTOs;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Spaces.Mappers;

public static class SpaceMapper
{
    public static SpaceDto ToDto(Space space)
    {
        var amenities = space.SpaceAmenities?
            .Where(sa => sa.Amenity != null)
            .Select(sa => new AmenityDto(sa.Amenity.Id, sa.Amenity.Name, sa.Amenity.Icon))
            .ToList() ?? new List<AmenityDto>();

        var media = space.Media?
            .OrderBy(m => m.DisplayOrder)
            .Select(m => new SpaceMediaDto(m.Id, m.Url, m.FileName, m.MediaType, m.DisplayOrder))
            .ToList() ?? new List<SpaceMediaDto>();

        return new SpaceDto(
            space.Id,
            space.Title,
            space.Description,
            space.PricePerHour,
            space.Location,
            space.City,
            space.Latitude,
            space.Longitude,
            space.Capacity,
            space.IsPublished,
            space.OwnerId,
            space.Type,
            space.CreatedAt,
            space.UpdatedAt,
            amenities,
            media);
    }
}
