using MediatR;
using SpaceRent.Application.Amenities.DTOs;
using SpaceRent.Application.Amenities.Queries;
using SpaceRent.Application.Interfaces;

namespace SpaceRent.Application.Amenities.Handlers;

public class GetAllAmenitiesQueryHandler : IRequestHandler<GetAllAmenitiesQuery, List<AmenityDto>>
{
    private readonly IAmenityRepository _amenityRepository;

    public GetAllAmenitiesQueryHandler(IAmenityRepository amenityRepository)
    {
        _amenityRepository = amenityRepository;
    }

    public async Task<List<AmenityDto>> Handle(GetAllAmenitiesQuery request, CancellationToken cancellationToken)
    {
        var amenities = await _amenityRepository.GetAllAsync(cancellationToken);
        return amenities.Select(a => new AmenityDto(a.Id, a.Name, a.Icon)).ToList();
    }
}
