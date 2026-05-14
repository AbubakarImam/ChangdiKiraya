using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceRent.Application.Bookings.Commands;
using SpaceRent.Application.Bookings.DTOs;
using SpaceRent.Application.Bookings.Queries;
using SpaceRent.Application.Common.Models;
using SpaceRent.Domain.Enums;

namespace SpaceRent.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<BookingDto>> CreateBooking([FromBody] CreateBookingRequest request)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

        var command = new CreateBookingCommand(request.SpaceId, userId, request.StartTime, request.EndTime);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetBookingDetails), new { id = result.Id }, result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PagedResult<BookingDto>>> GetBookings([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        return Ok(await _mediator.Send(new GetBookingsQuery(pageNumber, pageSize)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingDto>> GetBookingDetails(Guid id)
    {
        var booking = await _mediator.Send(new GetBookingByIdQuery(id));
        if (booking == null) return NotFound();

        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var currentUserId)) return Unauthorized();

        // Ensure requester is either the Tenant or the Space Owner
        if (booking.UserId != currentUserId && booking.SpaceOwnerId != currentUserId)
        {
            // Allow Admin bypass
            if (!User.IsInRole("Admin")) return Forbid();
        }

        return Ok(booking);
    }

    [HttpGet("my-bookings")]
    public async Task<ActionResult<PagedResult<BookingDto>>> GetMyBookings([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

        return Ok(await _mediator.Send(new GetUserBookingsQuery(userId, pageNumber, pageSize)));
    }

    [HttpGet("my-space-bookings")]
    public async Task<ActionResult<PagedResult<BookingDto>>> GetMySpaceBookings([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var ownerIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(ownerIdStr, out var ownerId)) return Unauthorized();

        return Ok(await _mediator.Send(new GetOwnerSpaceBookingsQuery(ownerId, pageNumber, pageSize)));
    }

    [HttpPatch("{id}/cancel")]
    public async Task<ActionResult> CancelBooking(Guid id)
    {
        var currentUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(currentUserIdStr, out var currentUserId)) return Unauthorized();

        var success = await _mediator.Send(new UpdateBookingStatusCommand(id, BookingStatus.Cancelled, currentUserId));
        if (!success) return BadRequest("Failed to cancel booking. You may not have permission or it does not exist.");
        
        return NoContent();
    }

    [HttpPatch("{id}/approve")]
    public async Task<ActionResult> ApproveBooking(Guid id)
    {
        var currentUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(currentUserIdStr, out var currentUserId)) return Unauthorized();

        var success = await _mediator.Send(new UpdateBookingStatusCommand(id, BookingStatus.Confirmed, currentUserId));
        if (!success) return BadRequest("Failed to approve booking. You may not have permission or it does not exist.");

        return NoContent();
    }

    [HttpPatch("{id}/reject")]
    public async Task<ActionResult> RejectBooking(Guid id)
    {
        var currentUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(currentUserIdStr, out var currentUserId)) return Unauthorized();

        var success = await _mediator.Send(new UpdateBookingStatusCommand(id, BookingStatus.Rejected, currentUserId));
        if (!success) return BadRequest("Failed to reject booking. You may not have permission or it does not exist.");

        return NoContent();
    }

    [HttpPatch("{id}/complete")]
    public async Task<ActionResult> CompleteBooking(Guid id)
    {
        var currentUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(currentUserIdStr, out var currentUserId)) return Unauthorized();

        var success = await _mediator.Send(new UpdateBookingStatusCommand(id, BookingStatus.Completed, currentUserId));
        if (!success) return BadRequest("Failed to mark booking as complete. You may not have permission or it does not exist.");

        return NoContent();
    }
}

public record CreateBookingRequest(Guid SpaceId, DateTime StartTime, DateTime EndTime);
