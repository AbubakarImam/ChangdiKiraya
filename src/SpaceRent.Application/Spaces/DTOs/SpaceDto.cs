using SpaceRent.Application.Amenities.DTOs;
using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Spaces.DTOs;

public record SpaceDto(
    Guid Id,
    string Title,
    string Description,
    decimal PricePerHour,
    string Location,
    string City,
    double Latitude,
    double Longitude,
    int Capacity,
    bool IsPublished,
    Guid OwnerId,
    SpaceType Type,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    List<AmenityDto> Amenities,
    List<SpaceMediaDto> Media);
