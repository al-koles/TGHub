using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using TGHub.Application.Common;
using TGHub.Application.Common.SessionStorage;
using TGHub.Application.Services.Base;
using TGHub.Application.Services.ChannelAdministrators;
using TGHub.Application.Services.Channels;
using TGHub.Application.Services.Jwt;
using TGHub.Application.Services.Posts;
using TGHub.Application.Services.Posts.Interfaces;
using TGHub.Application.Services.Posts.Jobs;
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
        services.AddTransient<IPostService, PostService>();
        services.AddTransient<IPostScheduleService, PostScheduleService>();
        services.AddTransient<IService<ChannelAdministrator>, ChannelAdministratorService>();

        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            q.AddJob<SendPostJob>(opt =>
            {
                opt.WithIdentity(SendPostJob.Key);
                opt.StoreDurably();
            });
        });
        services.AddQuartzServer(opt => { opt.WaitForJobsToComplete = true; });

        return services;
    }
}