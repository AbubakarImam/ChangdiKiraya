using Microsoft.EntityFrameworkCore;
using SpaceRent.Application.Common.Models;
using SpaceRent.Application.Interfaces;
using SpaceRent.Domain.Entities;
using SpaceRent.Infrastructure.Data;

namespace SpaceRent.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly SpaceRentDbContext _context;

    public BookingRepository(SpaceRentDbContext context)
    {
        _context = context;
    }

    public async Task<Booking> AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync(cancellationToken);
        return booking;
    }

    public async Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Include(b => b.Space)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<PagedResult<Booking>> GetPagedBookingsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Bookings
            .Include(b => b.Space)
            .Include(b => b.User)
            .OrderByDescending(b => b.StartTime);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PagedResult<Booking>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<PagedResult<Booking>> GetUserBookingsAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Bookings
            .Include(b => b.Space)
            .Include(b => b.User)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.StartTime);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PagedResult<Booking>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<PagedResult<Booking>> GetSpaceBookingsForOwnerAsync(Guid ownerId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Bookings
            .Include(b => b.Space)
            .Include(b => b.User)
            .Where(b => b.Space.OwnerId == ownerId)
            .OrderByDescending(b => b.StartTime);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PagedResult<Booking>(items, totalCount, pageNumber, pageSize);
    }
}
