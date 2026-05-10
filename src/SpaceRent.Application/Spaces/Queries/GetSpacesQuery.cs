using MediatR;
using SpaceRent.Application.Common.Models;
using SpaceRent.Application.Spaces.DTOs;
using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Spaces.Queries;

public record GetSpacesQuery(
    string? Location,
    decimal? MinPrice,
    decimal? MaxPrice,
    SpaceType? Type,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<PagedResult<SpaceDto>>;
