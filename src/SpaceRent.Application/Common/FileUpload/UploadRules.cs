namespace SpaceRent.Application.Common.FileUpload;

/// <summary>
/// Allowed file types and size limits per upload category.
/// </summary>
public static class UploadRules
{
    // ── Images ──────────────────────────────────────────────────────────────
    public static readonly string[] AllowedImageTypes =
        ["image/jpeg", "image/png", "image/webp", "image/gif"];

    public const long MaxImageSizeBytes = 10 * 1024 * 1024; // 10 MB

    // ── Thumbnails ───────────────────────────────────────────────────────────
    public static readonly string[] AllowedThumbnailTypes =
        ["image/jpeg", "image/png", "image/webp"];

    public const long MaxThumbnailSizeBytes = 5 * 1024 * 1024; // 5 MB

    // ── Videos ──────────────────────────────────────────────────────────────
    public static readonly string[] AllowedVideoTypes =
        ["video/mp4", "video/webm", "video/quicktime"];

    public const long MaxVideoSizeBytes = 200 * 1024 * 1024; // 200 MB

    // ── Profile Pictures ────────────────────────────────────────────────────
    public static readonly string[] AllowedProfilePictureTypes =
        ["image/jpeg", "image/png", "image/webp"];

    public const long MaxProfilePictureSizeBytes = 5 * 1024 * 1024; // 5 MB

    // ── ID Cards & Documents ─────────────────────────────────────────────────
    public static readonly string[] AllowedIdCardTypes =
        ["image/jpeg", "image/png", "application/pdf"];

    public const long MaxIdCardSizeBytes = 10 * 1024 * 1024; // 10 MB
}
