﻿using TGHub.Domain.Entities;

namespace TGHub.Application.Interfaces;

public interface ITgHubTelegramBotClient
{
    Task CreateOrUpdateChannelFromTg(long channelTgId);
    Task<int> SendPostAsync(Post post);
    Task<int> SendLotteryAsync(Lottery lottery);
    Task<int> SendLotteryResultAsync(Lottery lottery);
    Task UnBannUserAsync(long tgUserId, long tgChatId);
    Task BannUserAsync(long tgUserId, long tgChatId);
}