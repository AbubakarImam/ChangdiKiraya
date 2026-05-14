using MediatR;
using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Bookings.Commands;

public record UpdateBookingStatusCommand(Guid BookingId, BookingStatus Status, Guid CurrentUserId) : IRequest<bool>;
