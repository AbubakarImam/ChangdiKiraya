using MediatR;

namespace SpaceRent.Application.Amenities.Commands;

public record DeleteAmenityCommand(Guid Id) : IRequest<bool>;
