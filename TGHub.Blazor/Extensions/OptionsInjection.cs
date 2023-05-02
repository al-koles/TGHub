using TGHub.Application.Services.Jwt;
using TGHub.WebApiCore.Options;

namespace TGHub.Blazor.Extensions;

public static class OptionsInjection
{
    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelegramBotOptions>(configuration.GetSection(TelegramBotOptions.Alias));
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Alias));

        return services;
    }
}