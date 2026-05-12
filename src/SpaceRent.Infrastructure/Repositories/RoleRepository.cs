using Microsoft.EntityFrameworkCore;
using SpaceRent.Application.Interfaces;
using SpaceRent.Domain.Entities;
using SpaceRent.Infrastructure.Data;

namespace SpaceRent.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly SpaceRentDbContext _context;

    public RoleRepository(SpaceRentDbContext context)
    {
        _context = context;
    }

    public async Task<Role> AddAsync(Role role, CancellationToken cancellationToken = default)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync(cancellationToken);
        return role;
    }

    public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .Include(r => r.Users)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .Include(r => r.Users)
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }

    public async Task UpdateAsync(Role role, CancellationToken cancellationToken = default)
    {
        role.UpdatedAt = DateTime.UtcNow;
        _context.Roles.Update(role);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
            return false;

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .Include(r => r.Users)
            .ToListAsync(cancellationToken);
    }
}