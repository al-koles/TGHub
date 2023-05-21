using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Quartz;
using TGHub.Application.Common.Filtering;
using TGHub.Application.Common.Jobs;
using TGHub.Application.Interfaces;
using TGHub.Application.Services.Base;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Posts;

public class PostService : Service<Post>, IPostService
{
    private readonly ISchedulerFactory _schedulerFactory;

    public PostService(ITgHubDbContext dbContext, ISchedulerFactory schedulerFactory) : base(dbContext)
    {
        _schedulerFactory = schedulerFactory;
    }

    public override Task<List<Post>> ListAsync(FilterBase<Post>? filter = null)
    {
        var query = DbContext.Posts
            .AsNoTracking();

        if (filter == null)
        {
            return query.ToListAsync();
        }

        if (filter.Where != null)
        {
            query = query.Where(filter.Where);
        }

        if (filter is PostFilter postFilter)
        {
            if (postFilter.From.HasValue)
            {
                query = query.Where(p => p.ReleaseDateTime >= postFilter.From);
            }

            if (postFilter.To.HasValue)
            {
                query = query.Where(p => p.ReleaseDateTime <= postFilter.To);
            }

            if (postFilter.ChannelId.HasValue)
            {
                query = query.Where(p => p.Creator.ChannelId == postFilter.ChannelId);
            }

            if (postFilter.ChannelAdministratorId.HasValue)
            {
                query = query.Where(p =>
                    p.Creator.Channel.IsActive &&
                    p.Creator.Channel.Administrators.Any(a => a.AdministratorId == postFilter.ChannelAdministratorId &&
                                                              a.IsActive));
            }
        }

        return query.Sort(filter).ToListAsync();
    }

    public override Task<Post?> FirstOrDefaultAsync(Expression<Func<Post, bool>>? predicate = null)
    {
        var query = DbContext.Posts
            .Include(p => p.Attachments)
            .Include(p => p.Buttons)
            .Include(p => p.Creator)
            .ThenInclude(c => c.Administrator)
            .Include(p => p.Creator)
            .ThenInclude(p => p.Channel);

        return predicate == null
            ? query.FirstOrDefaultAsync()
            : query.FirstOrDefaultAsync(predicate);
    }

    public async Task ScheduleAsync(Post post)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        var triggerKey = new TriggerKey(post.Id.ToString(), "posts");
        var newTrigger = TriggerBuilder.Create()
            .WithIdentity(triggerKey)
            .ForJob(SendPostJob.Key)
            .StartAt(post.ReleaseDateTime)
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
}