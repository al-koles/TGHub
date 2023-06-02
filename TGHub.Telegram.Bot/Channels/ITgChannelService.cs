namespace TGHub.Telegram.Bot.Channels;

public interface ITgChannelService
{
    Task CreateOrUpdateChannelFromTgAsync(long channelTgId);
    Task UpdateCommentsGroupAsync(long commentsGroupTelegramId);
    Task UnBannUserAsync(long tgUserId, long tgChatId);
}