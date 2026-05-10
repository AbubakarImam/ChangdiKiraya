using MediatR;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Spaces.Commands;
using SpaceRent.Application.Spaces.DTOs;
using SpaceRent.Application.Spaces.Mappers;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Spaces.Handlers;

public class CreateSpaceCommandHandler : IRequestHandler<CreateSpaceCommand, SpaceDto>
{
    private readonly ISpaceRepository _spaceRepository;
    private readonly IAmenityRepository _amenityRepository;

    public CreateSpaceCommandHandler(ISpaceRepository spaceRepository, IAmenityRepository amenityRepository)
    {
        _spaceRepository = spaceRepository;
        _amenityRepository = amenityRepository;
    }

    public async Task<SpaceDto> Handle(CreateSpaceCommand request, CancellationToken cancellationToken)
    {
        var space = new Space
        {
            Title = request.Title,
            Description = request.Description,
            PricePerHour = request.PricePerHour,
            Location = request.Location,
            City = request.City,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Capacity = request.Capacity,
            OwnerId = request.OwnerId,
            Type = request.Type,
            IsPublished = false
        };

        // Attach amenities
        if (request.AmenityIds is { Count: > 0 })
        {
            var amenities = await _amenityRepository.GetByIdsAsync(request.AmenityIds, cancellationToken);
            space.SpaceAmenities = amenities.Select(a => new SpaceAmenity
            {
                AmenityId = a.Id,
                Amenity = a
            }).ToList();
        }

        var createdSpace = await _spaceRepository.AddAsync(space, cancellationToken);
        return SpaceMapper.ToDto(createdSpace);
    }
}
