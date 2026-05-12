using MediatR;
using SpaceRent.Application.Users.DTOs;

namespace SpaceRent.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IRequest<UserProfileDto?>;
