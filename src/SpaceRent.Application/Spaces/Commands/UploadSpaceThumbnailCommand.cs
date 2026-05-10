using MediatR;
using Microsoft.AspNetCore.Http;
using SpaceRent.Application.Spaces.DTOs;

namespace SpaceRent.Application.Spaces.Commands;

/// <summary>Upload a thumbnail to a space (replaces any existing thumbnail).</summary>
public record UploadSpaceThumbnailCommand(
    Guid SpaceId,
    IFormFile File) : IRequest<SpaceMediaDto>;
