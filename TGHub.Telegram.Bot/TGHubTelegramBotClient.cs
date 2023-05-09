using TGHub.Telegram.Bot.TelegramChannels;

namespace TGHub.Telegram.Bot;

internal class TgHubTelegramBotClient : ITgHubTelegramBotClient
{
    private readonly ITelegramChannelService _tgChannelService;

    public TgHubTelegramBotClient(ITelegramChannelService tgChannelService)
    {
        _tgChannelService = tgChannelService;
    }

    public Task CreateOrUpdateChannelFromTg(long channelTelegramId)
    {
        return _tgChannelService.CreateOrUpdateChannelFromTgAsync(channelTelegramId);
    }
}