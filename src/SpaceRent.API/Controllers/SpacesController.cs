using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpaceRent.Application.Common.Models;
using SpaceRent.Application.Spaces.Commands;
using SpaceRent.Application.Spaces.DTOs;
using SpaceRent.Application.Spaces.Queries;
using SpaceRent.Domain.Enums;

namespace SpaceRent.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpacesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpacesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a new space (for space owners).
    /// New spaces are created as unpublished by default.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(SpaceDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SpaceDto>> CreateSpace([FromBody] CreateSpaceCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetSpace), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update an existing space.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(SpaceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SpaceDto>> UpdateSpace(Guid id, [FromBody] UpdateSpaceCommand command)
    {
        if (id != command.Id)
            return BadRequest("Route id does not match body id.");

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Delete a space.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSpace(Guid id)
    {
        await _mediator.Send(new DeleteSpaceCommand(id));
        return NoContent();
    }

    /// <summary>
    /// Publish a space — makes it visible in search results.
    /// </summary>
    [HttpPatch("{id}/publish")]
    [ProducesResponseType(typeof(SpaceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SpaceDto>> PublishSpace(Guid id)
    {
        var result = await _mediator.Send(new PublishSpaceCommand(id));
        return Ok(result);
    }

    /// <summary>
    /// Unpublish a space — hides it from search results.
    /// </summary>
    [HttpPatch("{id}/unpublish")]
    [ProducesResponseType(typeof(SpaceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SpaceDto>> UnpublishSpace(Guid id)
    {
        var result = await _mediator.Send(new UnpublishSpaceCommand(id));
        return Ok(result);
    }

    /// <summary>
    /// Search published spaces with filters.
    /// Examples:
    ///   /api/spaces/search?city=lagos
    ///   /api/spaces/search?minPrice=5000&amp;maxPrice=50000
    ///   /api/spaces/search?type=Warehouse
    ///   /api/spaces/search?capacity=100
    ///   /api/spaces/search?amenityIds=guid1&amp;amenityIds=guid2
    ///   /api/spaces/search?lat=6.5&amp;lng=3.4&amp;radiusKm=15
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResult<SpaceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<SpaceDto>>> SearchSpaces(
        [FromQuery] string? city,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] SpaceType? type,
        [FromQuery] int? capacity,
        [FromQuery] List<Guid>? amenityIds,
        [FromQuery] double? lat,
        [FromQuery] double? lng,
        [FromQuery] double? radiusKm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new SearchSpacesQuery(city, minPrice, maxPrice, type, capacity, amenityIds, lat, lng, radiusKm, pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get a specific space by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SpaceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SpaceDto>> GetSpace(Guid id)
    {
        var result = await _mediator.Send(new GetSpaceByIdQuery(id));
        if (result is null)
            return NotFound();
        return Ok(result);
    }

    /// <summary>
    /// List all published spaces with basic filters and pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<SpaceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<SpaceDto>>> GetSpaces(
        [FromQuery] string? location,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] SpaceType? type,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetSpacesQuery(location, minPrice, maxPrice, type, pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
