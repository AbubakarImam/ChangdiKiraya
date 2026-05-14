using MediatR;
using SpaceRent.Application.Bookings.DTOs;

namespace SpaceRent.Application.Bookings.Queries;

public record GetBookingByIdQuery(Guid Id) : IRequest<BookingDto?>;
