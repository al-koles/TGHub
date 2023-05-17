using Microsoft.AspNetCore.Hosting;
using TGHub.Application.Interfaces;

namespace TGHub.ServerFileStorage;

internal class ServerFileStorageService : IFileStorage
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ServerFileStorageService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task UploadAsync(Stream stream, string fileName, string? directory = null)
    {
        var path = GetPath(fileName, directory);

        await using var fileStream = File.Create(path);
        await stream.CopyToAsync(fileStream);
    }

    public Task<Stream> DownloadAsync(string fileName, string? directory = null)
    {
        var path = GetPath(fileName, directory);

        return Task.FromResult((Stream)File.OpenRead(path));
    }

    public Task DeleteAsync(string fileName, string? directory = null)
    {
        var path = GetPath(fileName, directory);

        File.Delete(path);
        return Task.CompletedTask;
    }

    private string GetPath(string fileName, string? directory)
    {
        var path = _webHostEnvironment.WebRootPath;
        if (directory != null)
        {
            path = Path.Combine(path, directory);
            Directory.CreateDirectory(path);
        }

        path = Path.Combine(path, fileName);

        return path;
    }
}