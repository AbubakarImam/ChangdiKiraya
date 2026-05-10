using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Interfaces;

public interface IAmenityRepository
{
    Task<Amenity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Amenity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<Amenity>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default);
    Task<Amenity> AddAsync(Amenity amenity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Amenity amenity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Amenity amenity, CancellationToken cancellationToken = default);
}
