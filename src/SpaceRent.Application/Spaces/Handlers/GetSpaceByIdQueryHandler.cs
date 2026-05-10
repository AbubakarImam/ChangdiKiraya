using MediatR;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Spaces.DTOs;
using SpaceRent.Application.Spaces.Mappers;
using SpaceRent.Application.Spaces.Queries;

namespace SpaceRent.Application.Spaces.Handlers;

public class GetSpaceByIdQueryHandler : IRequestHandler<GetSpaceByIdQuery, SpaceDto?>
{
    private readonly ISpaceRepository _spaceRepository;

    public GetSpaceByIdQueryHandler(ISpaceRepository spaceRepository)
    {
        _spaceRepository = spaceRepository;
    }

    public async Task<SpaceDto?> Handle(GetSpaceByIdQuery request, CancellationToken cancellationToken)
    {
        var space = await _spaceRepository.GetByIdAsync(request.Id, cancellationToken);
        return space is null ? null : SpaceMapper.ToDto(space);
    }
}
