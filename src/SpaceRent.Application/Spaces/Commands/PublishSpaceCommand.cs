using MediatR;
using SpaceRent.Application.Spaces.DTOs;

namespace SpaceRent.Application.Spaces.Commands;

public record PublishSpaceCommand(Guid Id) : IRequest<SpaceDto>;
