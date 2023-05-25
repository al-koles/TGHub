using Microsoft.Extensions.Logging;
using Quartz;
using TGHub.Application.Common.Exceptions;
using TGHub.Application.Interfaces;
using TGHub.Application.Services.Lotteries.Interfaces;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Lotteries.Jobs;

public class SendLotteryJob : IJob
{
    public static readonly JobKey Key = new("SendLotteryJob");
    private readonly ILogger<SendLotteryJob> _logger;
    private readonly ILotteryService _lotteryService;
    private readonly ITgHubTelegramBotClient _telegramBotClient;

    public SendLotteryJob(ITgHubTelegramBotClient telegramBotClient, ILogger<SendLotteryJob> logger,
        ILotteryService lotteryService)
    {
        _telegramBotClient = telegramBotClient;
        _logger = logger;
        _lotteryService = lotteryService;
    }

    public int LotteryId { get; set; }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var lottery = await _lotteryService.FirstOrDefaultAsync(l => l.Id == LotteryId);
            if (lottery == null)
            {
                throw new NotFoundException(nameof(Lottery), LotteryId);
            }

            var lotteryTgId = await _telegramBotClient.SendLotteryAsync(lottery);

            lottery.LotteryTelegramId = lotteryTgId;
            await _lotteryService.UpdateAsync(lottery);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to send lottery ({0}) to channel", LotteryId);
        }
    }
}