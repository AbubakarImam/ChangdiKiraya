using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceRent.Application.Users.Commands.AssignUserRole;
using SpaceRent.Application.Users.Commands.RemoveProfilePicture;
using SpaceRent.Application.Users.Commands.UpdateProfile;
using SpaceRent.Application.Users.Commands.UpdateUserStatus;
using SpaceRent.Application.Users.Commands.UploadProfilePicture;
using SpaceRent.Application.Users.DTOs;
using SpaceRent.Application.Users.Queries.GetAllUsers;
using SpaceRent.Application.Users.Queries.GetProfile;
using SpaceRent.Application.Users.Queries.GetUserById;
using SpaceRent.Application.Users.Queries.GetUserReviews;
using SpaceRent.Application.Users.Queries.GetUserSpaces;
using SpaceRent.Domain.Enums;
using SpaceRent.Application.Spaces.DTOs;
using SpaceRent.Application.Reviews.DTOs;

namespace SpaceRent.API.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // --- Profile Endpoints --- //

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<UserProfileDto>> GetProfile()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

        var profile = await _mediator.Send(new GetProfileQuery(userId));
        if (profile == null) return NotFound();

        return Ok(profile);
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<ActionResult<UserProfileDto>> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

        var command = new UpdateProfileCommand(userId, request.Name, request.PhoneNumber, request.Bio, request.Address, request.City, request.State, request.Country);
        var profile = await _mediator.Send(command);
        
        if (profile == null) return NotFound();
        return Ok(profile);
    }

    [Authorize]
    [HttpPost("profile-picture")]
    public async Task<ActionResult<string>> UploadProfilePicture(IFormFile file)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

        if (file == null || file.Length == 0) return BadRequest("File is empty");

        using var stream = file.OpenReadStream();
        var command = new UploadProfilePictureCommand(userId, file.FileName, stream, file.ContentType);
        
        var url = await _mediator.Send(command);
        if (url == null) return BadRequest("Failed to upload picture");

        return Ok(new { Url = url });
    }

    [Authorize]
    [HttpDelete("profile-picture")]
    public async Task<ActionResult> RemoveProfilePicture()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

        var success = await _mediator.Send(new RemoveProfilePictureCommand(userId));
        if (!success) return BadRequest("Failed to remove picture");

        return NoContent();
    }

    // --- Public Endpoints --- //

    [HttpGet("{id}")]
    public async Task<ActionResult<UserProfileDto>> GetUserById(Guid id)
    {
        var profile = await _mediator.Send(new GetUserByIdQuery(id));
        if (profile == null) return NotFound();
        return Ok(profile);
    }

    [HttpGet("{id}/spaces")]
    public async Task<ActionResult<List<SpaceDto>>> GetUserSpaces(Guid id)
    {
        var spaces = await _mediator.Send(new GetUserSpacesQuery(id));
        return Ok(spaces);
    }

    [HttpGet("{id}/reviews")]
    public async Task<ActionResult<List<ReviewDto>>> GetUserReviews(Guid id)
    {
        var reviews = await _mediator.Send(new GetUserReviewsQuery(id));
        return Ok(reviews);
    }

    // --- Admin Endpoints --- //

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<PagedResult<UserProfileDto>>> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
    {
        var result = await _mediator.Send(new GetAllUsersQuery(pageNumber, pageSize, searchTerm));
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/role")]
    public async Task<ActionResult> AssignRole(Guid id, [FromBody] AssignRoleRequest request)
    {
        if (!Enum.TryParse<UserRole>(request.Role, true, out var roleEnum))
            return BadRequest("Invalid role");

        var success = await _mediator.Send(new AssignUserRoleCommand(id, roleEnum));
        if (!success) return BadRequest("Failed to assign role");

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/status")]
    public async Task<ActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusRequest request)
    {
        var success = await _mediator.Send(new UpdateUserStatusCommand(id, request.IsActive));
        if (!success) return BadRequest("Failed to update status");

        return NoContent();
    }
}

public record UpdateProfileRequest(string Name, string? PhoneNumber, string? Bio, string? Address, string? City, string? State, string? Country);
public record AssignRoleRequest(string Role);
public record UpdateStatusRequest(bool IsActive);
