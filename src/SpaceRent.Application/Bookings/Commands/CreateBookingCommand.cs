using MediatR;
using SpaceRent.Application.Bookings.DTOs;

namespace SpaceRent.Application.Bookings.Commands;

public record CreateBookingCommand(
    Guid SpaceId,
    Guid UserId,
    DateTime StartTime,
    DateTime EndTime) : IRequest<BookingDto>;
