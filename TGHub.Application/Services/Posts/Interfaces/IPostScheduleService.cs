using TGHub.Application.Services.Posts.Data;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Posts.Interfaces;

public interface IPostScheduleService
{
    Task ScheduleAsync(Post post);
    Task UnscheduleAsync(Post post);
    Task<PostSendStatus> GetSendStatusAsync(Post post);
}