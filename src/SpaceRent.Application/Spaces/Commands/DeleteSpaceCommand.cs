using MediatR;

namespace SpaceRent.Application.Spaces.Commands;

public record DeleteSpaceCommand(Guid Id) : IRequest<bool>;
