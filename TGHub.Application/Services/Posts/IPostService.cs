﻿using TGHub.Application.Services.Base;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Posts;

public interface IPostService : IService<Post>
{
    Task ScheduleAsync(Post post);
    Task UnscheduleAsync(Post post);
}