using MediatR;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Bookings.Commands;
using SpaceRent.Application.Bookings.DTOs;
using SpaceRent.Domain.Entities;
using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Bookings.Handlers;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingDto>
{
    private readonly IBookingRepository _bookingRepository;

    public CreateBookingCommandHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = new Booking
        {
            SpaceId = request.SpaceId,
            UserId = request.UserId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Status = BookingStatus.Pending
        };

        var createdBooking = await _bookingRepository.AddAsync(booking, cancellationToken);

        return new BookingDto(
            createdBooking.Id,
            createdBooking.SpaceId,
            createdBooking.UserId,
            createdBooking.StartTime,
            createdBooking.EndTime,
            createdBooking.Status);
    }
}
