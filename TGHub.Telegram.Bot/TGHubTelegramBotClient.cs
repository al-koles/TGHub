using Telegram.Bot;
using Telegram.Bot.Types;

namespace TGHub.Telegram.Bot;

public class TgHubTelegramBotClient
{
    private readonly ITelegramBotClient _telegramBotClient;

    public TgHubTelegramBotClient(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public async Task UpdateChannels()
    {
        // var channels = _telegramBotClient.GetChatAdministratorsAsync(new ChatId())
    }
}