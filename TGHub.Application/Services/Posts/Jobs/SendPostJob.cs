using Microsoft.Extensions.Logging;
using Quartz;
using TGHub.Application.Common.Exceptions;
using TGHub.Application.Interfaces;
using TGHub.Application.Services.Posts.Interfaces;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Posts.Jobs;

public class SendPostJob : IJob
{
    public static readonly JobKey Key = new("SendPostJob");
    private readonly ILogger<SendPostJob> _logger;
    private readonly IPostService _postService;
    private readonly ITgHubTelegramBotClient _telegramBotClient;

    public SendPostJob(ITgHubTelegramBotClient telegramBotClient, ILogger<SendPostJob> logger,
        IPostService postService)
    {
        _telegramBotClient = telegramBotClient;
        _logger = logger;
        _postService = postService;
    }

    public int PostId { get; set; }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var post = await _postService.FirstOrDefaultAsync(p => p.Id == PostId);
            if (post == null)
            {
                throw new NotFoundException(nameof(Post), PostId);
            }

            var postTgId = await _telegramBotClient.SendPostAsync(post);

            post.TelegramId = postTgId;
            await _postService.UpdateAsync(post);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to send post ({0}) to channel", PostId);
        }
    }
}