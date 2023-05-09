using Telegram.Bot;
using TGHub.Application.Common.Exceptions;
using TGHub.Telegram.Bot.TelegramChannels;

namespace TGHub.Telegram.Bot;

internal class TgHubTelegramBotClient : ITgHubTelegramBotClient
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ITelegramChannelService _tgChannelService;

    public TgHubTelegramBotClient(ITelegramBotClient telegramBotClient, ITelegramChannelService tgChannelService)
    {
        _telegramBotClient = telegramBotClient;
        _tgChannelService = tgChannelService;
    }

    public Task CreateOrUpdateChannelFromTg(long channelTgId)
    {
        return _tgChannelService.CreateOrUpdateChannelFromTgAsync(channelTgId);
    }
}