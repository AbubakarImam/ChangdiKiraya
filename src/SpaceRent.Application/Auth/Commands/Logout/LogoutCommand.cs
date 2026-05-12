using MediatR;

namespace SpaceRent.Application.Auth.Commands.Logout;

public record LogoutCommand(Guid UserId) : IRequest<bool>;
