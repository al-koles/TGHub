using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using TGHub.Telegram.Bot.Options;
using TGHub.Telegram.Bot.TelegramChannels;

namespace TGHub.Telegram.Bot;

public static class DependencyInjection
{
    public static IServiceCollection AddTelegramBotClient(this IServiceCollection services)
    {
        services.AddTransient<ITelegramBotClient, TelegramBotClient>(provider =>
        {
            var settings = provider.GetRequiredService<IOptionsSnapshot<TelegramBotOptions>>();
            return new TelegramBotClient(settings.Value.Token);
        });
        services.AddTransient<ITgHubTelegramBotClient, TgHubTelegramBotClient>();
        services.AddTransient<ITelegramChannelService, TelegramChannelService>();

        return services;
    }
}