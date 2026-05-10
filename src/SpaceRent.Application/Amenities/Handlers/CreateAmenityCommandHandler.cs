using MediatR;
using SpaceRent.Application.Amenities.Commands;
using SpaceRent.Application.Amenities.DTOs;
using SpaceRent.Application.Interfaces;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Amenities.Handlers;

public class CreateAmenityCommandHandler : IRequestHandler<CreateAmenityCommand, AmenityDto>
{
    private readonly IAmenityRepository _amenityRepository;

    public CreateAmenityCommandHandler(IAmenityRepository amenityRepository)
    {
        _amenityRepository = amenityRepository;
    }

    public async Task<AmenityDto> Handle(CreateAmenityCommand request, CancellationToken cancellationToken)
    {
        var amenity = new Amenity
        {
            Name = request.Name,
            Icon = request.Icon
        };

        var created = await _amenityRepository.AddAsync(amenity, cancellationToken);
        return new AmenityDto(created.Id, created.Name, created.Icon);
    }
}
