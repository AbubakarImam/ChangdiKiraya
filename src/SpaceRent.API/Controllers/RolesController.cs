using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceRent.Application.Roles.Commands.CreateRole;
using SpaceRent.Application.Roles.Commands.DeleteRole;
using SpaceRent.Application.Roles.Commands.UpdateRole;
using SpaceRent.Application.Roles.Commands.UpdateRolePermissions;
using SpaceRent.Application.Roles.DTOs;
using SpaceRent.Application.Roles.Queries.GetAllRoles;
using SpaceRent.Application.Roles.Queries.GetRoleById;

namespace SpaceRent.API.Controllers;

[ApiController]
[Route("roles")]
[Authorize(Roles = "Admin")]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<RoleDto>>> GetAllRoles()
    {
        var roles = await _mediator.Send(new GetAllRolesQuery());
        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetRoleById(Guid id)
    {
        var role = await _mediator.Send(new GetRoleByIdQuery(id));
        if (role == null) return NotFound();
        return Ok(role);
    }

    [HttpPost]
    public async Task<ActionResult<RoleDto>> CreateRole([FromBody] CreateRoleRequest request)
    {
        var role = await _mediator.Send(new CreateRoleCommand(request.Name, request.Description, request.Permissions));
        if (role == null) return BadRequest("Failed to create role");
        return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RoleDto>> UpdateRole(Guid id, [FromBody] UpdateRoleRequest request)
    {
        var role = await _mediator.Send(new UpdateRoleCommand(id, request.Name, request.Description, request.Permissions));
        if (role == null) return NotFound();
        return Ok(role);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRole(Guid id)
    {
        var success = await _mediator.Send(new DeleteRoleCommand(id));
        if (!success) return BadRequest("Failed to delete role");
        return NoContent();
    }

    [HttpPut("{id}/permissions")]
    public async Task<ActionResult<RoleDto>> UpdateRolePermissions(Guid id, [FromBody] UpdatePermissionsRequest request)
    {
        var role = await _mediator.Send(new UpdateRolePermissionsCommand(id, request.Permissions));
        if (role == null) return NotFound();
        return Ok(role);
    }
}

public record CreateRoleRequest(string Name, string? Description, string? Permissions);
public record UpdateRoleRequest(string Name, string? Description, string? Permissions);
public record UpdatePermissionsRequest(string Permissions);
