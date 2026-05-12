using Microsoft.EntityFrameworkCore;
using SpaceRent.Application.Interfaces;
using SpaceRent.Domain.Entities;
using SpaceRent.Infrastructure.Data;

namespace SpaceRent.Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly SpaceRentDbContext _context;

    public ReviewRepository(SpaceRentDbContext context)
    {
        _context = context;
    }

    public async Task<List<Review>> GetReviewsBySpaceOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Space)
            .Where(r => r.Space != null && r.Space.OwnerId == ownerId)
            .ToListAsync(cancellationToken);
    }
}
