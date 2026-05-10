using MediatR;
using SpaceRent.Application.Common.Models;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Spaces.DTOs;
using SpaceRent.Application.Spaces.Mappers;
using SpaceRent.Application.Spaces.Queries;

namespace SpaceRent.Application.Spaces.Handlers;

public class SearchSpacesQueryHandler : IRequestHandler<SearchSpacesQuery, PagedResult<SpaceDto>>
{
    private readonly ISpaceRepository _spaceRepository;

    public SearchSpacesQueryHandler(ISpaceRepository spaceRepository)
    {
        _spaceRepository = spaceRepository;
    }

    public async Task<PagedResult<SpaceDto>> Handle(SearchSpacesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _spaceRepository.SearchAsync(
            request.City,
            request.MinPrice,
            request.MaxPrice,
            request.Type,
            request.Capacity,
            request.AmenityIds,
            request.Lat,
            request.Lng,
            request.RadiusKm,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var dtos = items.Select(SpaceMapper.ToDto).ToList();
        return new PagedResult<SpaceDto>(dtos, totalCount, request.PageNumber, request.PageSize);
    }
}
