using MediatR;
using SpaceRent.Application.Amenities.DTOs;

namespace SpaceRent.Application.Amenities.Commands;

public record UpdateAmenityCommand(
    Guid Id,
    string Name,
    string Icon) : IRequest<AmenityDto>;
