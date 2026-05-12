using MediatR;
using SpaceRent.Application.Amenities.DTOs;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Spaces.DTOs;

namespace SpaceRent.Application.Users.Queries.GetUserSpaces;

public class GetUserSpacesQueryHandler : IRequestHandler<GetUserSpacesQuery, List<SpaceDto>>
{
    private readonly ISpaceRepository _spaceRepository;

    public GetUserSpacesQueryHandler(ISpaceRepository spaceRepository)
    {
        _spaceRepository = spaceRepository;
    }

    public async Task<List<SpaceDto>> Handle(GetUserSpacesQuery request, CancellationToken cancellationToken)
    {
        var spaces = await _spaceRepository.GetSpacesByOwnerIdAsync(request.UserId, cancellationToken);

        return spaces.Select(s => new SpaceDto(
            s.Id,
            s.Title,
            s.Description,
            s.PricePerHour,
            s.Location,
            s.City,
            s.Latitude,
            s.Longitude,
            s.Capacity,
            s.IsPublished,
            s.OwnerId,
            s.Type,
            s.CreatedAt,
            s.UpdatedAt,
            s.SpaceAmenities.Select(sa => new AmenityDto(sa.Amenity.Id, sa.Amenity.Name, sa.Amenity.Icon)).ToList(),
            s.Media.Select(m => new SpaceMediaDto(m.Id, m.Url, m.FileName ?? string.Empty, m.MediaType, m.DisplayOrder)).ToList()
        )).ToList();
    }
}
