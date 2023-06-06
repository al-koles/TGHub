using Microsoft.Extensions.DependencyInjection;

namespace TGHub.SpamModeration;

public static class DependencyInjection
{
    public static void AddSpamModeration(this IServiceCollection services)
    {
        services.AddSingleton<ISpamModerator, SpamModerator>();
    }
}