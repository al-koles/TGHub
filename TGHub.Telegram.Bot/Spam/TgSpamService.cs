using Telegram.Bot.Types;
using TGHub.Application.Services.Channels;
using TGHub.SpamModeration;

namespace TGHub.Telegram.Bot.Spam;

public class TgSpamService
{
    private readonly IChannelService _channelService;
    private readonly ISpamModerator _spamModerator;

    public TgSpamService(IChannelService channelService, ISpamModerator spamModerator)
    {
        _channelService = channelService;
        _spamModerator = spamModerator;
    }
    
    public async Task CheckMessageForSpamAsync(Message message)
    {
        var channel = await _channelService.FirstOrDefaultAsync(ch =>
            ch.IsActive && ch.OffensiveSpamOn && ch.LinkedChatTelegramId == message.Chat.Id);
        if (channel == null)
        {
            return;
        }

        var isNotSpam = await _spamModerator.ScanTextAsync(message.Text!);
        if (isNotSpam)
        {
            return;
        }
        
    }
}