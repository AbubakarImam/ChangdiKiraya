using MediatR;
using SpaceRent.Application.Bookings.DTOs;
using SpaceRent.Application.Bookings.Queries;
using SpaceRent.Application.Interfaces;

namespace SpaceRent.Application.Bookings.Handlers;

public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingDto?>
{
    private readonly IBookingRepository _bookingRepository;

    public GetBookingByIdQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<BookingDto?> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.Id, cancellationToken);
        if (booking == null) return null;

        return new BookingDto(
            booking.Id,
            booking.SpaceId,
            booking.UserId,
            booking.StartTime,
            booking.EndTime,
            booking.Status,
            booking.Space?.Title,
            booking.User?.Name,
            booking.Space?.OwnerId);
    }
}
