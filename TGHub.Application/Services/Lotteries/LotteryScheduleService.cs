using Quartz;
using TGHub.Application.Services.Lotteries.Data;
using TGHub.Application.Services.Lotteries.Interfaces;
using TGHub.Application.Services.Lotteries.Jobs;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Lotteries;

public class LotteryScheduleService : ILotteryScheduleService
{
    private const string LotteryTriggerGroup = "lotteries";
    private const string LotteryResultsTriggerGroup = "lottery-results";
    private readonly ISchedulerFactory _schedulerFactory;

    public LotteryScheduleService(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public async Task ScheduleLotteryAsync(Lottery lottery)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        var triggerKey = new TriggerKey(lottery.Id.ToString(), LotteryTriggerGroup);
        var newTrigger = TriggerBuilder.Create()
            .WithIdentity(triggerKey)
            .ForJob(SendLotteryJob.Key)
            .StartAt(new DateTimeOffset(lottery.StartDateTime, TimeSpan.Zero))
            .UsingJobData(new JobDataMap
                { { nameof(SendLotteryJob.LotteryId), lottery.Id } })
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

    public async Task ScheduleResultAsync(Lottery lottery)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        var triggerKey = new TriggerKey(lottery.Id.ToString(), LotteryResultsTriggerGroup);
        var newTrigger = TriggerBuilder.Create()
            .WithIdentity(triggerKey)
            .ForJob(SendLotteryResultJob.Key)
            .StartAt(new DateTimeOffset(lottery.EndDateTime, TimeSpan.Zero))
            .UsingJobData(new JobDataMap
                { { nameof(SendLotteryResultJob.LotteryId), lottery.Id } })
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

    public async Task UnscheduleLotteryAsync(Lottery lottery)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        var triggerKey = new TriggerKey(lottery.Id.ToString(), LotteryTriggerGroup);

        await scheduler.UnscheduleJob(triggerKey);
    }

    public async Task UnscheduleResultAsync(Lottery lottery)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        var triggerKey = new TriggerKey(lottery.Id.ToString(), LotteryResultsTriggerGroup);

        await scheduler.UnscheduleJob(triggerKey);
    }

    public async Task<LotterySendStatus> GetLotterySendStatusAsync(Lottery lottery)
    {
        if (lottery.LotteryTelegramId != null)
        {
            return LotterySendStatus.Sent;
        }

        if (lottery.StartDateTime < DateTime.UtcNow)
        {
            return LotterySendStatus.FailedToSend;
        }

        var scheduler = await _schedulerFactory.GetScheduler();
        var triggerKey = new TriggerKey(lottery.Id.ToString(), LotteryTriggerGroup);
        var trigger = await scheduler.GetTrigger(triggerKey);

        return trigger == null ? LotterySendStatus.NotScheduled : LotterySendStatus.Scheduled;
    }

    public async Task<LotterySendStatus> GetResultSendStatusAsync(Lottery lottery)
    {
        if (lottery.ResultTelegramId != null)
        {
            return LotterySendStatus.Sent;
        }

        if (lottery.EndDateTime < DateTime.UtcNow)
        {
            return LotterySendStatus.FailedToSend;
        }

        var scheduler = await _schedulerFactory.GetScheduler();
        var triggerKey = new TriggerKey(lottery.Id.ToString(), LotteryResultsTriggerGroup);
        var trigger = await scheduler.GetTrigger(triggerKey);

        return trigger == null ? LotterySendStatus.NotScheduled : LotterySendStatus.Scheduled;
    }
}