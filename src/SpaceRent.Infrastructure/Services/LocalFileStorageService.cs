using Microsoft.Extensions.Configuration;
using SpaceRent.Application.Interfaces;

namespace SpaceRent.Infrastructure.Services;

/// <summary>
/// Saves files to a configurable local folder.
/// Replace with an S3 or Azure Blob implementation for production.
/// </summary>
public class LocalFileStorageService : IFileStorageService
{
    private readonly string _baseUploadPath;
    private readonly string _baseUrl;

    public LocalFileStorageService(IConfiguration configuration)
    {
        _baseUploadPath = configuration["FileStorage:UploadPath"]
            ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        _baseUrl = configuration["FileStorage:BaseUrl"] ?? "http://localhost:5260/uploads";
    }

    public async Task<string> SaveAsync(
        Stream fileStream, string fileName, string contentType,
        string folder, CancellationToken cancellationToken = default)
    {
        var folderPath = Path.Combine(_baseUploadPath, folder);
        Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, fileName);

        await using var output = new FileStream(filePath, FileMode.Create);
        await fileStream.CopyToAsync(output, cancellationToken);

        return $"{_baseUrl}/{folder}/{fileName}";
    }

    public Task DeleteAsync(string fileUrl, CancellationToken cancellationToken = default)
    {
        var relativePath = fileUrl.Replace(_baseUrl, string.Empty)
            .TrimStart('/')
            .Replace('/', Path.DirectorySeparatorChar);

        var fullPath = Path.Combine(_baseUploadPath, relativePath);

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.CompletedTask;
    }
}
