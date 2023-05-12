namespace TGHub.Application.Interfaces;

public interface ITgHubTelegramBotClient
{
    Task CreateOrUpdateChannelFromTg(long channelTgId);
    Task<long> SendMessageToChannel(long channelTgId, string message);
}