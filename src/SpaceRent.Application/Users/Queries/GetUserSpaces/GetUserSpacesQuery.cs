using MediatR;
using SpaceRent.Application.Spaces.DTOs;

namespace SpaceRent.Application.Users.Queries.GetUserSpaces;

public record GetUserSpacesQuery(Guid UserId) : IRequest<List<SpaceDto>>;
