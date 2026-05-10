using MediatR;

namespace SpaceRent.Application.Spaces.Commands;

/// <summary>Delete a single media item from a space.</summary>
public record DeleteSpaceMediaCommand(
    Guid SpaceId,
    Guid MediaId) : IRequest<bool>;
