﻿using Microsoft.Extensions.Logging;
using Quartz;
using TGHub.Application.Common.Exceptions;
using TGHub.Application.Interfaces;
using TGHub.Application.Services.Lotteries.Interfaces;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Lotteries.Jobs;

public class SendLotteryResultJob : IJob
{
    public static readonly JobKey Key = new("SendLotteryResultJob");
    private readonly ILogger<SendLotteryResultJob> _logger;
    private readonly ILotteryService _lotteryService;
    private readonly ITgHubTelegramBotClient _telegramBotClient;

    public SendLotteryResultJob(ITgHubTelegramBotClient telegramBotClient, ILogger<SendLotteryResultJob> logger,
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

            if (lottery.LotteryTelegramId == null)
            {
                throw new Exception("Lottery wasn't send to Telegram so it's result also can't be sent");
            }

            await Task.Run(() => _lotteryService.SelectWinners(lottery));

            var lotteryTgId = await _telegramBotClient.SendLotteryResultAsync(lottery);

            lottery.ResultTelegramId = lotteryTgId;
            await _lotteryService.UpdateAsync(lottery);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to send lottery ({0}) result to channel", LotteryId);
        }
    }
}