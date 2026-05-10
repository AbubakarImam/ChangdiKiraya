using MediatR;
using SpaceRent.Application.Spaces.DTOs;

namespace SpaceRent.Application.Spaces.Commands;

public record UnpublishSpaceCommand(Guid Id) : IRequest<SpaceDto>;
