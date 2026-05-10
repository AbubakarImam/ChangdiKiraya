using MediatR;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Spaces.Commands;
using SpaceRent.Domain.Exceptions;

namespace SpaceRent.Application.Spaces.Handlers;

public class DeleteSpaceMediaCommandHandler : IRequestHandler<DeleteSpaceMediaCommand, bool>
{
    private readonly ISpaceMediaRepository _mediaRepository;
    private readonly IFileStorageService _fileStorage;

    public DeleteSpaceMediaCommandHandler(ISpaceMediaRepository mediaRepository, IFileStorageService fileStorage)
    {
        _mediaRepository = mediaRepository;
        _fileStorage = fileStorage;
    }

    public async Task<bool> Handle(DeleteSpaceMediaCommand request, CancellationToken cancellationToken)
    {
        var media = await _mediaRepository.GetByIdAsync(request.MediaId, cancellationToken)
            ?? throw new NotFoundException($"Media with id '{request.MediaId}' was not found.");

        if (media.SpaceId != request.SpaceId)
            throw new DomainException("Media does not belong to this space.");

        await _fileStorage.DeleteAsync(media.Url, cancellationToken);
        await _mediaRepository.DeleteAsync(media, cancellationToken);

        return true;
    }
}
