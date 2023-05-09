using TGHub.Application.Services.Base;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Channels;

public interface IChannelService : IService<Channel>
{
    Task MarkChannelInactiveIfExistAsync(long channelTelegramId);
}