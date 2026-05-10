using MediatR;
using SpaceRent.Application.Common.Models;
using SpaceRent.Application.Spaces.DTOs;
using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Spaces.Queries;

public record SearchSpacesQuery(
    string? City,
    decimal? MinPrice,
    decimal? MaxPrice,
    SpaceType? Type,
    int? Capacity,
    List<Guid>? AmenityIds,
    double? Lat,
    double? Lng,
    double? RadiusKm,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<PagedResult<SpaceDto>>;
