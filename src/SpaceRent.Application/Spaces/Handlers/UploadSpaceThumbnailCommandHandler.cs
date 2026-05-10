using MediatR;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Spaces.Commands;
using SpaceRent.Application.Spaces.DTOs;
using SpaceRent.Domain.Entities;
using SpaceRent.Domain.Enums;
using SpaceRent.Domain.Exceptions;

namespace SpaceRent.Application.Spaces.Handlers;

public class UploadSpaceThumbnailCommandHandler : IRequestHandler<UploadSpaceThumbnailCommand, SpaceMediaDto>
{
    private readonly ISpaceRepository _spaceRepository;
    private readonly ISpaceMediaRepository _mediaRepository;
    private readonly IFileStorageService _fileStorage;

    private static readonly string[] AllowedTypes = ["image/jpeg", "image/png", "image/webp"];

    public UploadSpaceThumbnailCommandHandler(
        ISpaceRepository spaceRepository,
        ISpaceMediaRepository mediaRepository,
        IFileStorageService fileStorage)
    {
        _spaceRepository = spaceRepository;
        _mediaRepository = mediaRepository;
        _fileStorage = fileStorage;
    }

    public async Task<SpaceMediaDto> Handle(UploadSpaceThumbnailCommand request, CancellationToken cancellationToken)
    {
        var space = await _spaceRepository.GetByIdAsync(request.SpaceId, cancellationToken)
            ?? throw new NotFoundException($"Space with id '{request.SpaceId}' was not found.");

        if (!AllowedTypes.Contains(request.File.ContentType.ToLower()))
            throw new DomainException("Thumbnail must be jpg, png, or webp.");

        if (request.File.Length > 5 * 1024 * 1024) // 5MB limit for thumbnail
            throw new DomainException("Thumbnail exceeds the 5MB size limit.");

        // Replace existing thumbnail if one exists
        var existing = await _mediaRepository.GetThumbnailBySpaceIdAsync(request.SpaceId, cancellationToken);
        if (existing != null)
        {
            await _fileStorage.DeleteAsync(existing.Url, cancellationToken);
            await _mediaRepository.DeleteAsync(existing, cancellationToken);
        }

        var safeFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";

        using var stream = request.File.OpenReadStream();
        var url = await _fileStorage.SaveAsync(stream, safeFileName, request.File.ContentType, "spaces/thumbnails", cancellationToken);

        var media = new SpaceMedia
        {
            SpaceId = request.SpaceId,
            Url = url,
            FileName = request.File.FileName,
            MediaType = MediaType.Thumbnail,
            DisplayOrder = 0
        };

        var saved = await _mediaRepository.AddAsync(media, cancellationToken);
        return new SpaceMediaDto(saved.Id, saved.Url, saved.FileName, saved.MediaType, saved.DisplayOrder);
    }
}
