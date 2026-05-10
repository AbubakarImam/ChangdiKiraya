using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpaceRent.Application.Spaces.Commands;
using SpaceRent.Application.Spaces.DTOs;

namespace SpaceRent.API.Controllers;

[ApiController]
[Route("api/spaces/{spaceId}/media")]
public class SpaceMediaController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpaceMediaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Upload a thumbnail for the space. Replaces any existing thumbnail.
    /// </summary>
    [HttpPost("thumbnail")]
    [ProducesResponseType(typeof(SpaceMediaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [RequestSizeLimit(5 * 1024 * 1024)] // 5MB
    public async Task<ActionResult<SpaceMediaDto>> UploadThumbnail(Guid spaceId, IFormFile file)
    {
        var result = await _mediator.Send(new UploadSpaceThumbnailCommand(spaceId, file));
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>
    /// Upload one or more images to the space.
    /// Supported formats: jpg, png, webp, gif. Max 10MB per file.
    /// </summary>
    [HttpPost("images")]
    [ProducesResponseType(typeof(List<SpaceMediaDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [RequestSizeLimit(50 * 1024 * 1024)] // 50MB total for batch
    public async Task<ActionResult<List<SpaceMediaDto>>> UploadImages(Guid spaceId, List<IFormFile> files)
    {
        if (files == null || files.Count == 0)
            return BadRequest("No files provided.");

        var result = await _mediator.Send(new UploadSpaceImagesCommand(spaceId, files));
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>
    /// Upload a video to the space. Replaces any existing video.
    /// Supported formats: mp4, webm, mov. Max 200MB.
    /// </summary>
    [HttpPost("video")]
    [ProducesResponseType(typeof(SpaceMediaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [RequestSizeLimit(200 * 1024 * 1024)] // 200MB
    public async Task<ActionResult<SpaceMediaDto>> UploadVideo(Guid spaceId, IFormFile file)
    {
        var result = await _mediator.Send(new UploadSpaceVideoCommand(spaceId, file));
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>
    /// Delete a specific media file (image, video, or thumbnail) from the space.
    /// </summary>
    [HttpDelete("{mediaId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMedia(Guid spaceId, Guid mediaId)
    {
        await _mediator.Send(new DeleteSpaceMediaCommand(spaceId, mediaId));
        return NoContent();
    }
}
