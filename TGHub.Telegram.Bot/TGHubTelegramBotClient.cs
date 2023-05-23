using TGHub.Application.Interfaces;
using TGHub.Domain.Entities;
using TGHub.Telegram.Bot.Channels;
using TGHub.Telegram.Bot.Posts;

namespace TGHub.Telegram.Bot;

internal class TgHubTelegramBotClient : ITgHubTelegramBotClient
{
    private readonly ITgSendService _tgSendService;
    private readonly ITgChannelService _tgTgChannelService;

    public TgHubTelegramBotClient(ITgChannelService tgTgChannelService, ITgSendService tgSendService)
    {
        _tgTgChannelService = tgTgChannelService;
        _tgSendService = tgSendService;
    }

    public Task CreateOrUpdateChannelFromTg(long channelTgId)
    {
        return _tgTgChannelService.CreateOrUpdateChannelFromTgAsync(channelTgId);
    }

    public Task<int> SendPostAsync(Post post)
    {
        return _tgSendService.SendPostAsync(post);
    }
}