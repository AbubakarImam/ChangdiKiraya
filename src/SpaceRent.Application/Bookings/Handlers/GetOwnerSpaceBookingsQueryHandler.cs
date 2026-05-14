using MediatR;
using SpaceRent.Application.Bookings.DTOs;
using SpaceRent.Application.Bookings.Queries;
using SpaceRent.Application.Common.Models;
using SpaceRent.Application.Interfaces;

namespace SpaceRent.Application.Bookings.Handlers;

public class GetOwnerSpaceBookingsQueryHandler : IRequestHandler<GetOwnerSpaceBookingsQuery, PagedResult<BookingDto>>
{
    private readonly IBookingRepository _bookingRepository;

    public GetOwnerSpaceBookingsQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<PagedResult<BookingDto>> Handle(GetOwnerSpaceBookingsQuery request, CancellationToken cancellationToken)
    {
        var pagedBookings = await _bookingRepository.GetSpaceBookingsForOwnerAsync(request.OwnerId, request.PageNumber, request.PageSize, cancellationToken);

        var dtos = pagedBookings.Items.Select(b => new BookingDto(
            b.Id,
            b.SpaceId,
            b.UserId,
            b.StartTime,
            b.EndTime,
            b.Status,
            b.Space?.Title,
            b.User?.Name,
            b.Space?.OwnerId)).ToList();

        return new PagedResult<BookingDto>(dtos, pagedBookings.TotalCount, pagedBookings.PageNumber, pagedBookings.PageSize);
    }
}
