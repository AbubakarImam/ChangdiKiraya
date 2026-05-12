using MediatR;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Roles.DTOs;

namespace SpaceRent.Application.Roles.Queries.GetAllRoles;

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<RoleDto>>
{
    private readonly IRoleRepository _roleRepository;

    public GetAllRolesQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<List<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAllAsync(cancellationToken);

        return roles.Select(r => new RoleDto(
            r.Id,
            r.Name ?? string.Empty,
            r.Description,
            r.Permissions,
            r.CreatedAt,
            r.UpdatedAt
        )).ToList();
    }
}
