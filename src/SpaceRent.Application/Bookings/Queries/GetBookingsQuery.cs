using MediatR;
using SpaceRent.Application.Bookings.DTOs;
using SpaceRent.Application.Common.Models;

namespace SpaceRent.Application.Bookings.Queries;

public record GetBookingsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PagedResult<BookingDto>>;
