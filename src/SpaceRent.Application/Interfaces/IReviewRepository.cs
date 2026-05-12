using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Interfaces;

public interface IReviewRepository
{
    Task<List<Review>> GetReviewsBySpaceOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
}
