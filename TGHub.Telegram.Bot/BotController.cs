using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TGHub.Application.Services.Channels;
using TGHub.Telegram.Bot.Channels;

namespace TGHub.Telegram.Bot;

[Route("api/[controller]")]
[ApiController]
public class BotController : ControllerBase
{
    private readonly IChannelService _channelService;
    private readonly ILogger<BotController> _logger;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ITgChannelService _tgChannelService;

    public BotController(ITelegramBotClient telegramBotClient, IChannelService channelService,
        ILogger<BotController> logger, ITgChannelService tgChannelService)
    {
        _telegramBotClient = telegramBotClient;
        _channelService = channelService;
        _logger = logger;
        _tgChannelService = tgChannelService;
    }

    [HttpPost]
    public async Task Post([FromBody] Update? update)
    {
        await HttpContext.Response.WriteAsync("Ok");
        if (update == null)
        {
            return;
        }

        try
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    await AnswerMessage(update);
                    break;
                case UpdateType.ChannelPost:
                    await AnswerPost(update);
                    break;
                case UpdateType.MyChatMember:
                    await MyChatMember(update);
                    break;
                case UpdateType.CallbackQuery:
                    await CallbackQuery(update);
                    break;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error");
        }
    }

    private async Task AnswerMessage(Update update)
    {
        var message = update.Message;
        if (message == null)
        {
            return;
        }

        if (message.Type == MessageType.Text)
        {
            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, message.Text,
                replyToMessageId: message.MessageId);
        }
        else
        {
            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "I understand only text",
                replyToMessageId: message.MessageId);
        }
    }

    private async Task AnswerPost(Update update)
    {
        var post = update.ChannelPost;
        if (post == null)
        {
            return;
        }

        await _telegramBotClient.SendTextMessageAsync(post.Chat.Id, post.Text,
            replyToMessageId: post.MessageId);
    }

    private async Task MyChatMember(Update update)
    {
        var member = update.MyChatMember;
        if (member == null)
        {
            return;
        }

        try
        {
            if (member.Chat.Type == ChatType.Channel)
            {
                if (member.NewChatMember.Status == ChatMemberStatus.Administrator)
                {
                    await _tgChannelService.CreateOrUpdateChannelFromTgAsync(member.Chat.Id);
                }
                else
                {
                    await _channelService.MarkChannelInactiveIfExistAsync(member.Chat.Id);
                }
            }
            else if (member.Chat.Type == ChatType.Supergroup)
            {
                await _tgChannelService.UpdateCommentsGroupAsync(member.Chat.Id);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error on member update");
        }
    }

    private async Task CallbackQuery(Update update)
    {
        
    }
}