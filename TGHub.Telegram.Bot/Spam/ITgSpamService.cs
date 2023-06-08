using Telegram.Bot.Types;

namespace TGHub.Telegram.Bot.Spam;

public interface ITgSpamService
{
    Task CheckMessageForSpamAsync(Message message, string text);
}