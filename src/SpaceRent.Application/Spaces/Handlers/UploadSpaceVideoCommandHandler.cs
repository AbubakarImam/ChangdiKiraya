using MediatR;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Spaces.Commands;
using SpaceRent.Application.Spaces.DTOs;
using SpaceRent.Domain.Entities;
using SpaceRent.Domain.Enums;
using SpaceRent.Domain.Exceptions;

namespace SpaceRent.Application.Spaces.Handlers;

public class UploadSpaceVideoCommandHandler : IRequestHandler<UploadSpaceVideoCommand, SpaceMediaDto>
{
    private readonly ISpaceRepository _spaceRepository;
    private readonly ISpaceMediaRepository _mediaRepository;
    private readonly IFileStorageService _fileStorage;

    private static readonly string[] AllowedTypes = ["video/mp4", "video/webm", "video/quicktime"];

    public UploadSpaceVideoCommandHandler(
        ISpaceRepository spaceRepository,
        ISpaceMediaRepository mediaRepository,
        IFileStorageService fileStorage)
    {
        _spaceRepository = spaceRepository;
        _mediaRepository = mediaRepository;
        _fileStorage = fileStorage;
    }

    public async Task<SpaceMediaDto> Handle(UploadSpaceVideoCommand request, CancellationToken cancellationToken)
    {
        var space = await _spaceRepository.GetByIdAsync(request.SpaceId, cancellationToken)
            ?? throw new NotFoundException($"Space with id '{request.SpaceId}' was not found.");

        if (!AllowedTypes.Contains(request.File.ContentType.ToLower()))
            throw new DomainException("Video must be mp4, webm, or mov.");

        if (request.File.Length > 200 * 1024 * 1024) // 200MB limit
            throw new DomainException("Video exceeds the 200MB size limit.");

        // Replace existing video if one exists
        var existing = await _mediaRepository.GetBySpaceIdAsync(request.SpaceId, cancellationToken);
        var existingVideo = existing.FirstOrDefault(m => m.MediaType == MediaType.Video);
        if (existingVideo != null)
        {
            await _fileStorage.DeleteAsync(existingVideo.Url, cancellationToken);
            await _mediaRepository.DeleteAsync(existingVideo, cancellationToken);
        }

        var safeFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";

        using var stream = request.File.OpenReadStream();
        var url = await _fileStorage.SaveAsync(stream, safeFileName, request.File.ContentType, "spaces/videos", cancellationToken);

        var media = new SpaceMedia
        {
            SpaceId = request.SpaceId,
            Url = url,
            FileName = request.File.FileName,
            MediaType = MediaType.Video,
            DisplayOrder = 0
        };

        var saved = await _mediaRepository.AddAsync(media, cancellationToken);
        return new SpaceMediaDto(saved.Id, saved.Url, saved.FileName, saved.MediaType, saved.DisplayOrder);
    }
}
