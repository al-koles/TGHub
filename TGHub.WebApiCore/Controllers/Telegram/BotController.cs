using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TGHub.WebApiCore.Controllers.Telegram;

[Route("api/[controller]")]
[ApiController]
public class BotController : ControllerBase
{
    private readonly ITelegramBotClient _telegramBotClient;

    public BotController(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        // var chat = await _telegramBotClient.GetChatAsync(1);
        var request = Request;
        if (update == null)
        {
            return Ok();
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
                    await ChatMember(update);
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return Ok();
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

    private async Task ChatMember(Update update)
    {
        var member = update.MyChatMember;
        if (member == null)
        {
            return;
        }

        try
        {
            var administrators = await _telegramBotClient.GetChatAdministratorsAsync(member.Chat.Id);
            await _telegramBotClient.SendTextMessageAsync(member.Chat.Id, "Hello world");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}