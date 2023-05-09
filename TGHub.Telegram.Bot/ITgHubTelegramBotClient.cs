namespace TGHub.Telegram.Bot;

public interface ITgHubTelegramBotClient
{
    Task CreateOrUpdateChannelFromTg(long channelTelegramId);
}