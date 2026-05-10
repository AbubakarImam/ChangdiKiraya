using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpaceRent.Application.Amenities.Commands;
using SpaceRent.Application.Amenities.DTOs;
using SpaceRent.Application.Amenities.Queries;

namespace SpaceRent.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AmenitiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AmenitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all available amenities.
    /// Use these IDs when creating or updating a space.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<AmenityDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AmenityDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllAmenitiesQuery());
        return Ok(result);
    }

    /// <summary>
    /// Create a new amenity (e.g. WiFi, Parking, AC, Security).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AmenityDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AmenityDto>> Create([FromBody] CreateAmenityCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update an existing amenity.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(AmenityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AmenityDto>> Update(Guid id, [FromBody] UpdateAmenityCommand command)
    {
        if (id != command.Id)
            return BadRequest("Route id does not match body id.");

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Delete an amenity.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteAmenityCommand(id));
        return NoContent();
    }
}
