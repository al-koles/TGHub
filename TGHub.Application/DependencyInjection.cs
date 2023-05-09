using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using TGHub.Application.Common;
using TGHub.Application.Common.SessionStorage;
using TGHub.Application.Services.Base;
using TGHub.Application.Services.ChannelAdministrators;
using TGHub.Application.Services.Channels;
using TGHub.Application.Services.Jwt;
using TGHub.Domain.Entities;

namespace TGHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IJwtService, JwtService>();
        services.AddScoped<LocalStorageProvider>();
        services.AddScoped<SessionStorageProvider>();
        services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

        services.AddTransient<IService<TgHubUser>, Service<TgHubUser>>();
        services.AddTransient<IChannelService, ChannelService>();
        services.AddTransient<IService<ChannelAdministrator>, ChannelAdministratorService>();
        services.AddTransient<IService<Post>, Service<Post>>();

        return services;
    }
}