using TGHub.Domain.Entities;

namespace TGHub.Application.Interfaces;

public interface ITgHubTelegramBotClient
{
    Task CreateOrUpdateChannelFromTg(long channelTgId);
    Task<int> SendPostAsync(Post post);
}