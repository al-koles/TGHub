namespace TGHub.Telegram.Bot.TelegramChannels;

public interface ITelegramChannelService
{
    Task CreateOrUpdateChannelFromTgAsync(long channelTgId);
    Task UpdateCommentsGroupAsync(long commentsGroupTelegramId);
}