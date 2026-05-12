using MediatR;

namespace SpaceRent.Application.Users.Commands.UpdateUserStatus;

public record UpdateUserStatusCommand(Guid UserId, bool IsActive) : IRequest<bool>;
