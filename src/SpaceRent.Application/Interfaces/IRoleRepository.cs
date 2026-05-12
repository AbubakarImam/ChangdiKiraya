using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Interfaces;

public interface IRoleRepository
{
    Task<Role> AddAsync(Role role, CancellationToken cancellationToken = default);
    Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task UpdateAsync(Role role, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default);
}