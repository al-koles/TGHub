using TGHub.Application.Interfaces;
using TGHub.Domain.Entities;
using TGHub.Telegram.Bot.Channels;
using TGHub.Telegram.Bot.Lotteries;
using TGHub.Telegram.Bot.Posts;

namespace TGHub.Telegram.Bot;

internal class TgHubTelegramBotClient : ITgHubTelegramBotClient
{
    private readonly ITgLotteryService _tgLotteryService;
    private readonly ITgPostService _tgPostService;
    private readonly ITgChannelService _tgTgChannelService;

    public TgHubTelegramBotClient(ITgChannelService tgTgChannelService, ITgPostService tgPostService,
        ITgLotteryService tgLotteryService)
    {
        _tgTgChannelService = tgTgChannelService;
        _tgPostService = tgPostService;
        _tgLotteryService = tgLotteryService;
    }

    public Task CreateOrUpdateChannelFromTg(long channelTgId)
    {
        return _tgTgChannelService.CreateOrUpdateChannelFromTgAsync(channelTgId);
    }

    public Task<int> SendPostAsync(Post post)
    {
        return _tgPostService.SendAsync(post);
    }

    public Task<int> SendLotteryAsync(Lottery lottery)
    {
        return _tgLotteryService.SendLotteryAsync(lottery);
    }

    public Task<int> SendLotteryResultAsync(Lottery lottery)
    {
        return _tgLotteryService.SendResultAsync(lottery);
    }
}