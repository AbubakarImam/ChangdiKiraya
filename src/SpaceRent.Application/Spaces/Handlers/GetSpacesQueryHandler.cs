using MediatR;
using SpaceRent.Application.Common.Models;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Spaces.DTOs;
using SpaceRent.Application.Spaces.Mappers;
using SpaceRent.Application.Spaces.Queries;

namespace SpaceRent.Application.Spaces.Handlers;

public class GetSpacesQueryHandler : IRequestHandler<GetSpacesQuery, PagedResult<SpaceDto>>
{
    private readonly ISpaceRepository _spaceRepository;

    public GetSpacesQueryHandler(ISpaceRepository spaceRepository)
    {
        _spaceRepository = spaceRepository;
    }

    public async Task<PagedResult<SpaceDto>> Handle(GetSpacesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _spaceRepository.GetFilteredAsync(
            request.Location,
            request.MinPrice,
            request.MaxPrice,
            request.Type,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var dtos = items.Select(SpaceMapper.ToDto).ToList();
        return new PagedResult<SpaceDto>(dtos, totalCount, request.PageNumber, request.PageSize);
    }
}
