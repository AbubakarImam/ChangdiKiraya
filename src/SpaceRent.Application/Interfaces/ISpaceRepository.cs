using SpaceRent.Domain.Entities;
using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Interfaces;

public interface ISpaceRepository
{
    Task<Space?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Space?> GetByIdWithAmenitiesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Space>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<(List<Space> Items, int TotalCount)> GetFilteredAsync(string? location, decimal? minPrice, decimal? maxPrice, SpaceType? type, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<(List<Space> Items, int TotalCount)> SearchAsync(string? city, decimal? minPrice, decimal? maxPrice, SpaceType? type, int? capacity, List<Guid>? amenityIds, double? lat, double? lng, double? radiusKm, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<Space> AddAsync(Space space, CancellationToken cancellationToken = default);
    Task UpdateAsync(Space space, CancellationToken cancellationToken = default);
    Task DeleteAsync(Space space, CancellationToken cancellationToken = default);
}
