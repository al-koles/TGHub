namespace TGHub.Application.Common.Extensions;

public static class StreamExtensions
{
    public static async Task CopyToWithProgressAsync(
        this Stream source, 
        Stream destination, 
        IProgress<long> progress, 
        int bufferSize = 0x1000, 
        CancellationToken cancellationToken = default)
    {
        long totalRead = 0;
        var buffer = new byte[bufferSize];
        int bytesRead;
        while ((bytesRead = await source.ReadAsync(buffer, cancellationToken)) != 0)
        {
            cancellationToken.ThrowIfCancellationRequested();
            totalRead += bytesRead;
            await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken);
            progress.Report(totalRead);
        }
    }
}