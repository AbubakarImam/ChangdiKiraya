using MediatR;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Spaces.Commands;
using SpaceRent.Application.Spaces.DTOs;
using SpaceRent.Application.Spaces.Mappers;

namespace SpaceRent.Application.Spaces.Handlers;

public class PublishSpaceCommandHandler : IRequestHandler<PublishSpaceCommand, SpaceDto>
{
    private readonly ISpaceRepository _spaceRepository;

    public PublishSpaceCommandHandler(ISpaceRepository spaceRepository)
    {
        _spaceRepository = spaceRepository;
    }

    public async Task<SpaceDto> Handle(PublishSpaceCommand request, CancellationToken cancellationToken)
    {
        var space = await _spaceRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new SpaceRent.Domain.Exceptions.NotFoundException($"Space with id '{request.Id}' was not found.");

        space.IsPublished = true;
        space.UpdatedAt = DateTime.UtcNow;

        await _spaceRepository.UpdateAsync(space, cancellationToken);
        return SpaceMapper.ToDto(space);
    }
}
