using Microsoft.EntityFrameworkCore;
using SpaceRent.Application.Interfaces;
using SpaceRent.Domain.Entities;
using SpaceRent.Domain.Enums;
using SpaceRent.Infrastructure.Data;

namespace SpaceRent.Infrastructure.Repositories;

public class SpaceMediaRepository : ISpaceMediaRepository
{
    private readonly SpaceRentDbContext _context;

    public SpaceMediaRepository(SpaceRentDbContext context)
    {
        _context = context;
    }

    public async Task<List<SpaceMedia>> GetBySpaceIdAsync(Guid spaceId, CancellationToken cancellationToken = default)
    {
        return await _context.SpaceMedia
            .Where(m => m.SpaceId == spaceId)
            .OrderBy(m => m.MediaType)
            .ThenBy(m => m.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<SpaceMedia?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SpaceMedia.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<SpaceMedia?> GetThumbnailBySpaceIdAsync(Guid spaceId, CancellationToken cancellationToken = default)
    {
        return await _context.SpaceMedia
            .FirstOrDefaultAsync(m => m.SpaceId == spaceId && m.MediaType == MediaType.Thumbnail, cancellationToken);
    }

    public async Task<bool> HasThumbnailAsync(Guid spaceId, CancellationToken cancellationToken = default)
    {
        return await _context.SpaceMedia
            .AnyAsync(m => m.SpaceId == spaceId && m.MediaType == MediaType.Thumbnail, cancellationToken);
    }

    public async Task<int> GetNextDisplayOrderAsync(Guid spaceId, MediaType mediaType, CancellationToken cancellationToken = default)
    {
        var maxOrder = await _context.SpaceMedia
            .Where(m => m.SpaceId == spaceId && m.MediaType == mediaType)
            .Select(m => (int?)m.DisplayOrder)
            .MaxAsync(cancellationToken);

        return (maxOrder ?? -1) + 1;
    }

    public async Task<SpaceMedia> AddAsync(SpaceMedia media, CancellationToken cancellationToken = default)
    {
        _context.SpaceMedia.Add(media);
        await _context.SaveChangesAsync(cancellationToken);
        return media;
    }

    public async Task DeleteAsync(SpaceMedia media, CancellationToken cancellationToken = default)
    {
        _context.SpaceMedia.Remove(media);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
