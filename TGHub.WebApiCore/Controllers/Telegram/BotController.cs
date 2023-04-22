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
        var request = Request;
        if (update == null)
        {
            return Ok();
        }

        var message = update.Message;
        if (message == null)
        {
            return Ok();
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

        return Ok();
    }
}