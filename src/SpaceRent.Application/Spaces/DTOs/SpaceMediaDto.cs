using SpaceRent.Domain.Enums;

namespace SpaceRent.Application.Spaces.DTOs;

public record SpaceMediaDto(
    Guid Id,
    string Url,
    string FileName,
    MediaType MediaType,
    int DisplayOrder);
