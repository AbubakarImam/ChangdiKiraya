using Microsoft.EntityFrameworkCore;
using SpaceRent.Application.Interfaces;
using SpaceRent.Domain.Entities;
using SpaceRent.Infrastructure.Data;

namespace SpaceRent.Infrastructure.Repositories;

public class AmenityRepository : IAmenityRepository
{
    private readonly SpaceRentDbContext _context;

    public AmenityRepository(SpaceRentDbContext context)
    {
        _context = context;
    }

    public async Task<Amenity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Amenities.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<List<Amenity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Amenities.OrderBy(a => a.Name).ToListAsync(cancellationToken);
    }

    public async Task<List<Amenity>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await _context.Amenities.Where(a => ids.Contains(a.Id)).ToListAsync(cancellationToken);
    }

    public async Task<Amenity> AddAsync(Amenity amenity, CancellationToken cancellationToken = default)
    {
        _context.Amenities.Add(amenity);
        await _context.SaveChangesAsync(cancellationToken);
        return amenity;
    }

    public async Task UpdateAsync(Amenity amenity, CancellationToken cancellationToken = default)
    {
        _context.Amenities.Update(amenity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Amenity amenity, CancellationToken cancellationToken = default)
    {
        _context.Amenities.Remove(amenity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
