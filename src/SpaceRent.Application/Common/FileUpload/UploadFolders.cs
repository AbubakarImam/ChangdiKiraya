namespace SpaceRent.Application.Common.FileUpload;

/// <summary>
/// Centralised folder paths for all uploaded files.
/// Inject this pattern anywhere you upload — never hardcode folder strings.
/// </summary>
public static class UploadFolders
{
    // Space media
    public const string SpaceThumbnails = "spaces/thumbnails";
    public const string SpaceImages     = "spaces/images";
    public const string SpaceVideos     = "spaces/videos";

    // User media
    public const string UserProfilePictures = "users/profile-pictures";
    public const string UserIdCards         = "users/id-cards";
    public const string UserDocuments       = "users/documents";
}
