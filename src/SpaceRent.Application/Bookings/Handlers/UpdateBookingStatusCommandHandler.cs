using MediatR;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Bookings.Commands;
using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Bookings.Handlers;

public class UpdateBookingStatusCommandHandler : IRequestHandler<UpdateBookingStatusCommand, bool>
{
    private readonly IBookingRepository _bookingRepository;

    public UpdateBookingStatusCommandHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<bool> Handle(UpdateBookingStatusCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);
        if (booking == null)
            return false;

        // Authorization checks
        if (request.Status == BookingStatus.Cancelled)
            if (booking.UserId != request.CurrentUserId && booking.Space.OwnerId != request.CurrentUserId)
                return false;

        if (request.Status == BookingStatus.Confirmed || request.Status == BookingStatus.Rejected)
            if (booking.Space.OwnerId != request.CurrentUserId)
                return false;

        // Note: Completed could be marked by either, depending on business rules, but let's allow either
        if (request.Status == BookingStatus.Completed)
            if (booking.UserId != request.CurrentUserId && booking.Space.OwnerId != request.CurrentUserId)
                return false;

        booking.Status = request.Status;
        await _bookingRepository.UpdateAsync(booking, cancellationToken);

        return true;
    }
}
