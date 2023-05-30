using Microsoft.EntityFrameworkCore;
using TGHub.Application;
using TGHub.Application.Services.Lotteries.Data;
using TGHub.Application.Services.Lotteries.Interfaces;
using TGHub.Application.Services.Posts.Data;
using TGHub.Application.Services.Posts.Interfaces;

namespace TGHub.Blazor.Extensions;

public static class WebApplicationExtensions
{
    public static async Task MigrateDbContextIfNecessaryAsync<TDbContext>(this WebApplication app)
        where TDbContext : DbContext
    {
        using var scope = app.Services.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TDbContext>>();
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

        try
        {
            if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
            {
                logger.LogInformation($"Migrating database associated with context {typeof(TDbContext).Name}");
                await dbContext.Database.MigrateAsync();
                logger.LogInformation($"Migrated database associated with context {typeof(TDbContext).Name}");
            }
            else
            {
                logger.LogInformation(
                    $"Not found pending migrations associated with context {typeof(TDbContext).Name}");
            }
        }
        catch (Exception e)
        {
            logger.LogError(e,
                $"An error occurred while migrating the database used on context {typeof(TDbContext).Name}");
        }
    }

    public static void UseLocalization(this WebApplication app)
    {
        var supportedCultures = ApplicationConstants.SupportedCultures;
        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        app.UseRequestLocalization(localizationOptions);
    }

    public static async Task SchedulePostsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("Getting future posts to schedule");
        try
        {
            var postService = scope.ServiceProvider.GetRequiredService<IPostService>();
            var postsToSchedule = await postService.ListAsync(new PostFilter
            {
                From = DateTime.UtcNow,
                Where = p => p.TelegramId == null
            });

            logger.LogInformation($"Found {postsToSchedule.Count} posts to schedule");
            logger.LogInformation("Scheduling posts");

            var postScheduleService = scope.ServiceProvider.GetRequiredService<IPostScheduleService>();
            await Task.WhenAll(postsToSchedule.Select(p => postScheduleService.ScheduleAsync(p)));

            logger.LogInformation("Posts scheduled");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while scheduling posts");
        }
    }

    public static async Task ScheduleLotteriesAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("Getting future lotteries to schedule");
        try
        {
            var postService = scope.ServiceProvider.GetRequiredService<ILotteryService>();
            var lotteriesToSchedule = await postService.ListAsync(new LotteryFilter
            {
                From = DateTime.UtcNow,
                Where = l => l.LotteryTelegramId == null || l.ResultTelegramId == null
            });

            logger.LogInformation($"Found {lotteriesToSchedule.Count} lotteries to schedule");
            logger.LogInformation("Scheduling lotteries");

            var lotteryScheduleService = scope.ServiceProvider.GetRequiredService<ILotteryScheduleService>();
            await Task.WhenAll(lotteriesToSchedule.Select(async l =>
            {
                if (l.LotteryTelegramId == null && l.StartDateTime > DateTime.UtcNow)
                {
                    await lotteryScheduleService.ScheduleLotteryAsync(l);
                }

                if (l.ResultTelegramId == null && l.EndDateTime > DateTime.UtcNow)
                {
                    await lotteryScheduleService.ScheduleResultAsync(l);
                }
            }));

            logger.LogInformation("Lotteries scheduled");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while scheduling lotteries");
        }
    }
}