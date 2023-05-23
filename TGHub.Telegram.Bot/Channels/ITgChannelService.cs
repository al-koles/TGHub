namespace TGHub.Telegram.Bot.Channels;

public interface ITgChannelService
{
    Task CreateOrUpdateChannelFromTgAsync(long channelTgId);
    Task UpdateCommentsGroupAsync(long commentsGroupTelegramId);
}