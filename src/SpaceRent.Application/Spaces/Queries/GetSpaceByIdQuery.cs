using MediatR;
using SpaceRent.Application.Spaces.DTOs;

namespace SpaceRent.Application.Spaces.Queries;

public record GetSpaceByIdQuery(Guid Id) : IRequest<SpaceDto?>;
