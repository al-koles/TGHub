using TGHub.Domain.Entities;

namespace TGHub.Telegram.Bot.Posts;

public interface ITgSendService
{
    Task<int> SendPostAsync(Post post);
}