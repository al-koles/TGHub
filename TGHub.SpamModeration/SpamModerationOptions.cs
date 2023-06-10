namespace TGHub.SpamModeration;

public class SpamModerationOptions
{
    public const string Alias = "SpamModeration";
    public string SubscriptionKey { get; set; } = null!;
    public string Endpoint { get; set; } = null!;
}