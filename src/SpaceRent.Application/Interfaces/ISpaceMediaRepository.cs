using SpaceRent.Domain.Entities;
using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Interfaces;

public interface ISpaceMediaRepository
{
    Task<List<SpaceMedia>> GetBySpaceIdAsync(Guid spaceId, CancellationToken cancellationToken = default);
    Task<SpaceMedia?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SpaceMedia?> GetThumbnailBySpaceIdAsync(Guid spaceId, CancellationToken cancellationToken = default);
    Task<SpaceMedia> AddAsync(SpaceMedia media, CancellationToken cancellationToken = default);
    Task DeleteAsync(SpaceMedia media, CancellationToken cancellationToken = default);
    Task<bool> HasThumbnailAsync(Guid spaceId, CancellationToken cancellationToken = default);
    Task<int> GetNextDisplayOrderAsync(Guid spaceId, MediaType mediaType, CancellationToken cancellationToken = default);
}
