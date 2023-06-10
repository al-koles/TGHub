using Quartz;
using TGHub.Application.Services.Posts.Data;
using TGHub.Application.Services.Posts.Interfaces;
using TGHub.Application.Services.Posts.Jobs;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Posts;

public class PostScheduleService : IPostScheduleService
{
    private readonly ISchedulerFactory _schedulerFactory;

    public PostScheduleService(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public async Task ScheduleAsync(Post post)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        var triggerKey = new TriggerKey(post.Id.ToString(), "posts");
        var newTrigger = TriggerBuilder.Create()
            .WithIdentity(triggerKey)
            .ForJob(SendPostJob.Key)
            .StartAt(new DateTimeOffset(post.ReleaseDateTime, TimeSpan.Zero))
            .UsingJobData(new JobDataMap
                { { nameof(SendPostJob.PostId), post.Id } })
            .Build();

        var trigger = await scheduler.GetTrigger(triggerKey);
        if (trigger == null)
        {
            await scheduler.ScheduleJob(newTrigger);
        }
        else
        {
            await scheduler.RescheduleJob(triggerKey, newTrigger);
        }
    }

    public async Task UnscheduleAsync(Post post)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        var triggerKey = new TriggerKey(post.Id.ToString(), "posts");

        await scheduler.UnscheduleJob(triggerKey);
    }

    public async Task<PostSendStatus> GetSendStatusAsync(Post post)
    {
        if (post.TelegramId != null)
        {
            return PostSendStatus.Sent;
        }

        if (post.ReleaseDateTime < DateTime.UtcNow)
        {
            return PostSendStatus.FailedToSend;
        }

        var scheduler = await _schedulerFactory.GetScheduler();
        var triggerKey = new TriggerKey(post.Id.ToString(), "posts");
        var trigger = await scheduler.GetTrigger(triggerKey);

        return trigger == null ? PostSendStatus.NotScheduled : PostSendStatus.Scheduled;
    }
}