using MediatR;
using SpaceRent.Application.Amenities.DTOs;

namespace SpaceRent.Application.Amenities.Commands;

public record CreateAmenityCommand(
    string Name,
    string Icon) : IRequest<AmenityDto>;
