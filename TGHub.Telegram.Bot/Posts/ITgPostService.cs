using TGHub.Domain.Entities;

namespace TGHub.Telegram.Bot.Posts;

public interface ITgPostService
{
    Task<int> SendAsync(Post post);
}