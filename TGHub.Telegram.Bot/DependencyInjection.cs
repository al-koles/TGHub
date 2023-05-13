using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using TGHub.Application.Interfaces;
using TGHub.Telegram.Bot.Channels;
using TGHub.Telegram.Bot.Options;
using TGHub.Telegram.Bot.Posts;

namespace TGHub.Telegram.Bot;

public static class DependencyInjection
{
    public static void AddTelegramBotClient(this IServiceCollection services)
    {
        services.AddTransient<ITelegramBotClient, TelegramBotClient>(provider =>
        {
            var settings = provider.GetRequiredService<IOptionsSnapshot<TelegramBotOptions>>();
            return new TelegramBotClient(settings.Value.Token);
        });
        services.AddTransient<ITgHubTelegramBotClient, TgHubTelegramBotClient>();
        services.AddTransient<ITgChannelService, TgChannelService>();
        services.AddTransient<ITgSendService, TgSendService>();
    }
}