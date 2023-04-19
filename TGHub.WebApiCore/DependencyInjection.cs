using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using TGHub.WebApiCore.Options;

namespace TGHub.WebApiCore;

public static class DependencyInjection
{
    public static IServiceCollection AddTelegramBotClient(this IServiceCollection services)
    {
        services.AddTransient<ITelegramBotClient, TelegramBotClient>(provider =>
        {
            var settings = provider.GetRequiredService<IOptionsSnapshot<TelegramBotOptions>>();
            return new TelegramBotClient(settings.Value.Token);
        });

        return services;
    }
}