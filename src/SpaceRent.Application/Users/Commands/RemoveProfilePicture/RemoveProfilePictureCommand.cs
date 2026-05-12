using MediatR;

namespace SpaceRent.Application.Users.Commands.RemoveProfilePicture;

public record RemoveProfilePictureCommand(Guid UserId) : IRequest<bool>;
