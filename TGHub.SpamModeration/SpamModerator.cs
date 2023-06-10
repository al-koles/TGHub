using System.Text;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Extensions.Options;

namespace TGHub.SpamModeration;

public class SpamModerator : ISpamModerator
{
    private readonly IDisposable _optionsListener;
    private ContentModeratorClient _client;
    private SpamModerationOptions _options;

    public SpamModerator(IOptionsMonitor<SpamModerationOptions> options)
    {
        _options = options.CurrentValue;
        _client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(_options.SubscriptionKey));
        _client.Endpoint = _options.Endpoint;

        _optionsListener = options.OnChange(opt =>
        {
            _options = opt;
            _client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(_options.SubscriptionKey));
            _client.Endpoint = _options.Endpoint;
        });
    }

    public async Task<bool> ScanTextIsNotSpamAsync(string text, double minScore = 0.7)
    {
        using var textStream = new MemoryStream(Encoding.UTF8.GetBytes(text));
        var screen = await _client.TextModeration.ScreenTextAsync("text/plain", textStream, classify: true);
        return screen.Classification == null ||
               (screen.Classification.Category1.Score <= minScore &&
                screen.Classification.Category2.Score <= minScore &&
                screen.Classification.Category3.Score <= minScore);
    }

    public void Dispose()
    {
        _client.Dispose();
        _optionsListener.Dispose();
    }
}