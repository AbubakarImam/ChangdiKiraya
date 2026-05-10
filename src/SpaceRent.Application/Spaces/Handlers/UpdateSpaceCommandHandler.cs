using MediatR;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Spaces.Commands;
using SpaceRent.Application.Spaces.DTOs;
using SpaceRent.Application.Spaces.Mappers;
using SpaceRent.Domain.Entities;
using SpaceRent.Domain.Exceptions;

namespace SpaceRent.Application.Spaces.Handlers;

public class UpdateSpaceCommandHandler : IRequestHandler<UpdateSpaceCommand, SpaceDto>
{
    private readonly ISpaceRepository _spaceRepository;
    private readonly IAmenityRepository _amenityRepository;

    public UpdateSpaceCommandHandler(ISpaceRepository spaceRepository, IAmenityRepository amenityRepository)
    {
        _spaceRepository = spaceRepository;
        _amenityRepository = amenityRepository;
    }

    public async Task<SpaceDto> Handle(UpdateSpaceCommand request, CancellationToken cancellationToken)
    {
        var space = await _spaceRepository.GetByIdWithAmenitiesAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Space with id '{request.Id}' was not found.");

        space.Title = request.Title;
        space.Description = request.Description;
        space.PricePerHour = request.PricePerHour;
        space.Location = request.Location;
        space.City = request.City;
        space.Latitude = request.Latitude;
        space.Longitude = request.Longitude;
        space.Capacity = request.Capacity;
        space.Type = request.Type;
        space.UpdatedAt = DateTime.UtcNow;

        // Replace amenities
        space.SpaceAmenities.Clear();
        if (request.AmenityIds is { Count: > 0 })
        {
            var amenities = await _amenityRepository.GetByIdsAsync(request.AmenityIds, cancellationToken);
            foreach (var amenity in amenities)
            {
                space.SpaceAmenities.Add(new SpaceAmenity
                {
                    SpaceId = space.Id,
                    AmenityId = amenity.Id,
                    Amenity = amenity
                });
            }
        }

        await _spaceRepository.UpdateAsync(space, cancellationToken);
        return SpaceMapper.ToDto(space);
    }
}
