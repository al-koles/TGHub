namespace TGHub.SpamModeration;

public interface ISpamModerator : IDisposable
{
    Task<bool> ScanTextIsNotSpamAsync(string text, double minScore = 0.7);
}