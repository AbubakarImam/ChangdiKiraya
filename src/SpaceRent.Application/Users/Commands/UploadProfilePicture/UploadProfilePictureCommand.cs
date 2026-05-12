using MediatR;

namespace SpaceRent.Application.Users.Commands.UploadProfilePicture;

public record UploadProfilePictureCommand(Guid UserId, string FileName, Stream FileStream, string ContentType) : IRequest<string?>;
