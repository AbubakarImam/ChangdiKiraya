using Microsoft.AspNetCore.Http;
using SpaceRent.Domain.Exceptions;

namespace SpaceRent.Application.Common.FileUpload;

/// <summary>
/// Reusable validation helper for any file upload.
/// Throws <see cref="DomainException"/> on validation failure.
/// </summary>
public static class FileUploadValidator
{
    public static void Validate(IFormFile file, string[] allowedTypes, long maxSizeBytes)
    {
        if (file == null || file.Length == 0)
            throw new DomainException("No file was provided or the file is empty.");

        if (!allowedTypes.Contains(file.ContentType.ToLower()))
        {
            var allowed = string.Join(", ", allowedTypes);
            throw new DomainException($"'{file.ContentType}' is not a supported file type. Allowed: {allowed}.");
        }

        if (file.Length > maxSizeBytes)
        {
            var limitMb = maxSizeBytes / 1024 / 1024;
            throw new DomainException($"File '{file.FileName}' exceeds the {limitMb}MB size limit.");
        }
    }

    public static void ValidateAll(IEnumerable<IFormFile> files, string[] allowedTypes, long maxSizeBytes)
    {
        foreach (var file in files)
            Validate(file, allowedTypes, maxSizeBytes);
    }

    /// <summary>Generate a unique, safe filename while preserving the original extension.</summary>
    public static string GenerateSafeFileName(IFormFile file)
        => $"{Guid.NewGuid()}{Path.GetExtension(file.FileName).ToLowerInvariant()}";
}
