using TGHub.Application.Services.Jwt;
using TGHub.SpamModeration;
using TGHub.Telegram.Bot.Options;

namespace TGHub.Blazor.Extensions;

public static class OptionsInjection
{
    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelegramBotOptions>(configuration.GetSection(TelegramBotOptions.Alias));
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Alias));
        services.Configure<SpamModerationOptions>(configuration.GetSection(SpamModerationOptions.Alias));

        return services;
    }
}