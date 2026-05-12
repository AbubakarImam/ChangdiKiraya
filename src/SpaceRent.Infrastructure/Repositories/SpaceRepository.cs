using Microsoft.EntityFrameworkCore;
using SpaceRent.Application.Interfaces;
using SpaceRent.Domain.Entities;
using SpaceRent.Domain.Enums;
using SpaceRent.Infrastructure.Data;

namespace SpaceRent.Infrastructure.Repositories;

public class SpaceRepository : ISpaceRepository
{
    private readonly SpaceRentDbContext _context;

    public SpaceRepository(SpaceRentDbContext context)
    {
        _context = context;
    }

    public async Task<Space?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Spaces
            .Include(s => s.Owner)
            .Include(s => s.SpaceAmenities).ThenInclude(sa => sa.Amenity)
            .Include(s => s.Media.OrderBy(m => m.DisplayOrder))
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Space?> GetByIdWithAmenitiesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Spaces
            .Include(s => s.SpaceAmenities).ThenInclude(sa => sa.Amenity)
            .Include(s => s.Media.OrderBy(m => m.DisplayOrder))
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<List<Space>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Spaces
            .Include(s => s.SpaceAmenities).ThenInclude(sa => sa.Amenity)
            .ToListAsync(cancellationToken);
    }

    public async Task<(List<Space> Items, int TotalCount)> GetFilteredAsync(
        string? location, decimal? minPrice, decimal? maxPrice, SpaceType? type,
        int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Spaces
            .Include(s => s.SpaceAmenities).ThenInclude(sa => sa.Amenity)
            .Where(s => s.IsPublished)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(location))
            query = query.Where(s => s.Location.Contains(location));

        if (minPrice.HasValue)
            query = query.Where(s => s.PricePerHour >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(s => s.PricePerHour <= maxPrice.Value);

        if (type.HasValue)
            query = query.Where(s => (s.Type & type.Value) == type.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(List<Space> Items, int TotalCount)> SearchAsync(
        string? city, decimal? minPrice, decimal? maxPrice, SpaceType? type,
        int? capacity, List<Guid>? amenityIds, double? lat, double? lng, double? radiusKm,
        int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Spaces
            .Include(s => s.SpaceAmenities).ThenInclude(sa => sa.Amenity)
            .Where(s => s.IsPublished)
            .AsQueryable();

        // Filter by city (case-insensitive contains)
        if (!string.IsNullOrWhiteSpace(city))
            query = query.Where(s => s.City.ToLower().Contains(city.ToLower()));

        // Filter by price range
        if (minPrice.HasValue)
            query = query.Where(s => s.PricePerHour >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(s => s.PricePerHour <= maxPrice.Value);

        // Filter by space type (bitwise flag match)
        if (type.HasValue)
            query = query.Where(s => (s.Type & type.Value) == type.Value);

        // Filter by minimum capacity
        if (capacity.HasValue)
            query = query.Where(s => s.Capacity >= capacity.Value);

        // Filter by amenities — space must have ALL requested amenities
        if (amenityIds is { Count: > 0 })
        {
            foreach (var amenityId in amenityIds)
            {
                query = query.Where(s => s.SpaceAmenities.Any(sa => sa.AmenityId == amenityId));
            }
        }

        // Filter by proximity (lat/lng with radius using bounding box)
        if (lat.HasValue && lng.HasValue)
        {
            var radius = radiusKm ?? 10.0;
            var latRad = lat.Value * Math.PI / 180.0;
            var latDelta = radius / 111.0;
            var lngDelta = radius / (111.0 * Math.Cos(latRad));

            query = query.Where(s =>
                s.Latitude >= lat.Value - latDelta &&
                s.Latitude <= lat.Value + latDelta &&
                s.Longitude >= lng.Value - lngDelta &&
                s.Longitude <= lng.Value + lngDelta);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Space> AddAsync(Space space, CancellationToken cancellationToken = default)
    {
        _context.Spaces.Add(space);
        await _context.SaveChangesAsync(cancellationToken);
        return space;
    }

    public async Task UpdateAsync(Space space, CancellationToken cancellationToken = default)
    {
        _context.Spaces.Update(space);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Space space, CancellationToken cancellationToken = default)
    {
        _context.Spaces.Remove(space);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Space>> GetSpacesByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        return await _context.Spaces
            .Include(s => s.SpaceAmenities)
                .ThenInclude(sa => sa.Amenity)
            .Include(s => s.Media)
            .Where(s => s.OwnerId == ownerId)
            .ToListAsync(cancellationToken);
    }
}
