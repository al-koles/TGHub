using TGHub.Domain.Entities;

namespace TGHub.Telegram.Bot.Posts;

public interface ITgSendService
{
    Task<long> SendPostAsync(Post post);
}