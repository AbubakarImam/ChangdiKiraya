using MediatR;
using SpaceRent.Application.Users.DTOs;

namespace SpaceRent.Application.Users.Queries.GetProfile;

public record GetProfileQuery(Guid UserId) : IRequest<UserProfileDto?>;
