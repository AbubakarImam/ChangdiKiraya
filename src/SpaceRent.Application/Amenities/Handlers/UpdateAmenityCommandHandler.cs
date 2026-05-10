using MediatR;
using SpaceRent.Application.Amenities.Commands;
using SpaceRent.Application.Amenities.DTOs;
using SpaceRent.Application.Interfaces;
using SpaceRent.Domain.Exceptions;

namespace SpaceRent.Application.Amenities.Handlers;

public class UpdateAmenityCommandHandler : IRequestHandler<UpdateAmenityCommand, AmenityDto>
{
    private readonly IAmenityRepository _amenityRepository;

    public UpdateAmenityCommandHandler(IAmenityRepository amenityRepository)
    {
        _amenityRepository = amenityRepository;
    }

    public async Task<AmenityDto> Handle(UpdateAmenityCommand request, CancellationToken cancellationToken)
    {
        var amenity = await _amenityRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Amenity with id '{request.Id}' was not found.");

        amenity.Name = request.Name;
        amenity.Icon = request.Icon;

        await _amenityRepository.UpdateAsync(amenity, cancellationToken);
        return new AmenityDto(amenity.Id, amenity.Name, amenity.Icon);
    }
}
