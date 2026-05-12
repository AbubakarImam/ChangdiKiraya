using MediatR;
using SpaceRent.Application.Common.FileUpload;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Spaces.Commands;
using SpaceRent.Application.Spaces.DTOs;
using SpaceRent.Domain.Entities;
using SpaceRent.Domain.Enums;
using SpaceRent.Domain.Exceptions;

namespace SpaceRent.Application.Spaces.Handlers;

public class UploadSpaceImagesCommandHandler : IRequestHandler<UploadSpaceImagesCommand, List<SpaceMediaDto>>
{
    private readonly ISpaceRepository _spaceRepository;
    private readonly ISpaceMediaRepository _mediaRepository;
    private readonly IFileStorageService _fileStorage;

    public UploadSpaceImagesCommandHandler(
        ISpaceRepository spaceRepository,
        ISpaceMediaRepository mediaRepository,
        IFileStorageService fileStorage)
    {
        _spaceRepository = spaceRepository;
        _mediaRepository = mediaRepository;
        _fileStorage = fileStorage;
    }

    public async Task<List<SpaceMediaDto>> Handle(UploadSpaceImagesCommand request, CancellationToken cancellationToken)
    {
        var space = await _spaceRepository.GetByIdAsync(request.SpaceId, cancellationToken)
            ?? throw new NotFoundException($"Space with id '{request.SpaceId}' was not found.");

        // Validate all files upfront before saving any
        FileUploadValidator.ValidateAll(request.Files, UploadRules.AllowedImageTypes, UploadRules.MaxImageSizeBytes);

        var results = new List<SpaceMediaDto>();

        foreach (var file in request.Files)
        {
            var safeFileName = FileUploadValidator.GenerateSafeFileName(file);
            var order = await _mediaRepository.GetNextDisplayOrderAsync(request.SpaceId, MediaType.Image, cancellationToken);

            using var stream = file.OpenReadStream();
            var url = await _fileStorage.SaveAsync(stream, safeFileName, file.ContentType, UploadFolders.SpaceImages, cancellationToken);

            var media = new SpaceMedia
            {
                SpaceId = request.SpaceId,
                Url = url,
                FileName = file.FileName,
                MediaType = MediaType.Image,
                DisplayOrder = order
            };

            var saved = await _mediaRepository.AddAsync(media, cancellationToken);
            results.Add(new SpaceMediaDto(saved.Id, saved.Url, saved.FileName, saved.MediaType, saved.DisplayOrder));
        }

        return results;
    }
}
