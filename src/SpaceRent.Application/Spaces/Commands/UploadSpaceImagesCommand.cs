using MediatR;
using Microsoft.AspNetCore.Http;
using SpaceRent.Application.Spaces.DTOs;
using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Spaces.Commands;

/// <summary>Upload one or more images to a space.</summary>
public record UploadSpaceImagesCommand(
    Guid SpaceId,
    List<IFormFile> Files) : IRequest<List<SpaceMediaDto>>;
