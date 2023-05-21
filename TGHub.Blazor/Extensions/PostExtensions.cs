using TGHub.Domain.Entities;

namespace TGHub.Blazor.Extensions;

public static class PostExtensions
{
    public static PostSendStatus GetSendStatus(this Post post)
    {
        if (post.TelegramId != null)
        {
            return PostSendStatus.Success;
        }

        return post.ReleaseDateTime < DateTime.UtcNow ? PostSendStatus.Error : PostSendStatus.Pending;
    }
}

public enum PostSendStatus
{
    Pending,
    Success,
    Error
}