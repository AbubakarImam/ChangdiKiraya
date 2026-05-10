using MediatR;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Spaces.Commands;

namespace SpaceRent.Application.Spaces.Handlers;

public class DeleteSpaceCommandHandler : IRequestHandler<DeleteSpaceCommand, bool>
{
    private readonly ISpaceRepository _spaceRepository;

    public DeleteSpaceCommandHandler(ISpaceRepository spaceRepository)
    {
        _spaceRepository = spaceRepository;
    }

    public async Task<bool> Handle(DeleteSpaceCommand request, CancellationToken cancellationToken)
    {
        var space = await _spaceRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new SpaceRent.Domain.Exceptions.NotFoundException($"Space with id '{request.Id}' was not found.");

        await _spaceRepository.DeleteAsync(space, cancellationToken);
        return true;
    }
}
