using Microsoft.AspNetCore.Hosting;
using TGHub.Application.Common.Extensions;
using TGHub.Application.Interfaces;

namespace TGHub.ServerFileStorage;

internal class ServerFileStorageService : IFileStorage
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ServerFileStorageService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
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

    public async Task UploadAsync(Stream stream, string fileName, string? directory = null,
        IProgress<long>? progress = null, CancellationToken cancellationToken = default)
    {
        var path = GetPath(fileName, directory);

        await using var fileStream = File.Create(path);
        if (progress == null)
        {
            await stream.CopyToAsync(fileStream, cancellationToken);
        }
        else
        {
            await stream.CopyToWithProgressAsync(fileStream, progress, cancellationToken: cancellationToken);
        }
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