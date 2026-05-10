using Microsoft.EntityFrameworkCore;
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
}
