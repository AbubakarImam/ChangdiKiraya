using MediatR;
using Microsoft.AspNetCore.Http;
using SpaceRent.Application.Spaces.DTOs;

namespace SpaceRent.Application.Spaces.Commands;

/// <summary>Upload a single video to a space (replaces any existing video).</summary>
public record UploadSpaceVideoCommand(
    Guid SpaceId,
    IFormFile File) : IRequest<SpaceMediaDto>;
