using SpaceRent.Application.Common.Models;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Interfaces;

public interface IBookingRepository
{
    Task<Booking> AddAsync(Booking booking, CancellationToken cancellationToken = default);
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default);
    Task<PagedResult<Booking>> GetPagedBookingsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<Booking>> GetUserBookingsAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<Booking>> GetSpaceBookingsForOwnerAsync(Guid ownerId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
