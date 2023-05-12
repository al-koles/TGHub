using Telegram.Bot;
using Telegram.Bot.Types;
using TGHub.Application.Interfaces;
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

    public async Task<long> SendMessageToChannel(long channelTgId, string message)
    {
        var post = await _telegramBotClient.SendTextMessageAsync(new ChatId(channelTgId), message);
        return post.MessageId;
    }
}