namespace SpaceRent.Application.Interfaces;

public interface IFileStorageService
{
    /// <summary>
    /// Saves a file and returns its publicly accessible URL.
    /// </summary>
    Task<string> SaveAsync(Stream fileStream, string fileName, string contentType, string folder, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a file by its URL or storage key.
    /// </summary>
    Task DeleteAsync(string fileUrl, CancellationToken cancellationToken = default);
}
