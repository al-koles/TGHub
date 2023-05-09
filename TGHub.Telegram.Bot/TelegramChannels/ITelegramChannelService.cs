namespace TGHub.Telegram.Bot.TelegramChannels;

public interface ITelegramChannelService
{
    Task CreateOrUpdateChannelFromTgAsync(long channelTelegramId);
    Task UpdateCommentsGroupAsync(long commentsGroupTelegramId);
}