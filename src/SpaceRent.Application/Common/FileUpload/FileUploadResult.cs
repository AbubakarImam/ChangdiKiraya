namespace SpaceRent.Application.Common.FileUpload;

/// <summary>
/// Returned by any file upload operation across the platform.
/// </summary>
public record FileUploadResult(
    string Url,
    string FileName,
    string ContentType,
    long SizeBytes);
