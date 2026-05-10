using MediatR;
using SpaceRent.Application.Spaces.DTOs;
using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Spaces.Commands;

public record UpdateSpaceCommand(
    Guid Id,
    string Title,
    string Description,
    decimal PricePerHour,
    string Location,
    string City,
    double Latitude,
    double Longitude,
    int Capacity,
    List<Guid> AmenityIds,
    SpaceType Type) : IRequest<SpaceDto>;
