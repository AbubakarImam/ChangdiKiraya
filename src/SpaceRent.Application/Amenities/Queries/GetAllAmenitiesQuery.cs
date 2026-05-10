using MediatR;
using SpaceRent.Application.Amenities.DTOs;

namespace SpaceRent.Application.Amenities.Queries;

public record GetAllAmenitiesQuery() : IRequest<List<AmenityDto>>;
