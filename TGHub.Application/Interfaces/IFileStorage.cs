namespace TGHub.Application.Interfaces;

public interface IFileStorage
{
    Task UploadAsync(Stream stream, string fileName, string? directory = null);
    Task<Stream> DownloadAsync(string fileName, string? directory = null);
    Task DeleteAsync(string fileName, string? directory = null);
}