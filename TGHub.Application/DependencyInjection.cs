using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using TGHub.Application.Common;
using TGHub.Application.Common.SessionStorage;
using TGHub.Application.Services.Base;
using TGHub.Application.Services.ChannelAdministrators;
using TGHub.Application.Services.Channels;
using TGHub.Application.Services.Jwt;
using TGHub.Application.Services.Lotteries;
using TGHub.Application.Services.Lotteries.Interfaces;
using TGHub.Application.Services.Lotteries.Jobs;
using TGHub.Application.Services.Posts;
using TGHub.Application.Services.Posts.Interfaces;
using TGHub.Application.Services.Posts.Jobs;
using TGHub.Application.Services.Spam;
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
        services.AddTransient<ISpamService, SpamService>();
        services.AddTransient<IPostService, PostService>();
        services.AddTransient<IPostScheduleService, PostScheduleService>();
        services.AddTransient<IService<ChannelAdministrator>, ChannelAdministratorService>();
        services.AddTransient<ILotteryService, LotteryService>();
        services.AddTransient<ILotteryScheduleService, LotteryScheduleService>();

        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            q.AddJob<SendPostJob>(opt =>
            {
                opt.WithIdentity(SendPostJob.Key);
                opt.StoreDurably();
            });
            q.AddJob<SendLotteryJob>(opt =>
            {
                opt.WithIdentity(SendLotteryJob.Key);
                opt.StoreDurably();
            });
            q.AddJob<SendLotteryResultJob>(opt =>
            {
                opt.WithIdentity(SendLotteryResultJob.Key);
                opt.StoreDurably();
            });
        });
        services.AddQuartzServer(opt => { opt.WaitForJobsToComplete = true; });

        return services;
    }
}