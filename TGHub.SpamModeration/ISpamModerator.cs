namespace TGHub.SpamModeration;

public interface ISpamModerator : IDisposable
{
    Task<bool> ScanTextAsync(string text, double minScore = 0.7);
}