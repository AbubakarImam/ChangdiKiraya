using MediatR;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Auth.Queries.GetMe;

public record GetMeQuery(Guid UserId) : IRequest<User?>;
