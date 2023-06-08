using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using TGHub.Application;
using TGHub.Application.Services.Channels;
using TGHub.Application.Services.Spam;
using TGHub.Application.Services.Spam.Models;
using TGHub.Domain.Entities;
using TGHub.Domain.Enums;
using TGHub.SpamModeration;

namespace TGHub.Telegram.Bot.Spam;

public class TgSpamService : ITgSpamService
{
    private readonly IChannelService _channelService;
    private readonly ILogger<TgSpamService> _logger;
    private readonly ISpamModerator _spamModerator;
    private readonly ISpamService _spamService;
    private readonly ITelegramBotClient _telegramBotClient;

    public TgSpamService(IChannelService channelService, ISpamModerator spamModerator,
        ITelegramBotClient telegramBotClient, ISpamService spamService, ILogger<TgSpamService> logger)
    {
        _channelService = channelService;
        _spamModerator = spamModerator;
        _telegramBotClient = telegramBotClient;
        _spamService = spamService;
        _logger = logger;
    }

    public async Task CheckMessageForSpamAsync(Message message, string text)
    {
        var channel = await _channelService.FirstOrDefaultAsync(ch =>
            ch.IsActive && ch.LinkedChatTelegramId == message.Chat.Id);
        if (channel == null)
        {
            return;
        }

        var spamContext = string.Empty;
        var spamType = (SpamMessageType)0;

        var isOffensiveSpam = false;
        if (channel.OffensiveSpamOn)
        {
            isOffensiveSpam = !await _spamModerator.ScanTextIsNotSpamAsync(text);
            if (isOffensiveSpam)
            {
                spamType = spamType | SpamMessageType.OffensiveLanguage;
            }
        }

        var isListSpam = false;
        if (channel.ListSpamOn)
        {
            var spamWordsFound = await _spamService.FindSpamWordsAsync(text, channel.Id);
            isListSpam = spamWordsFound.Any();
            spamContext = string.Join(", ", spamWordsFound);
            spamType = spamType | SpamMessageType.SpamWordFound;
        }

        if (isOffensiveSpam || isListSpam)
        {
            try
            {
                await _telegramBotClient.DeleteMessageAsync(message.Chat.Id, message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to delete spam message '{0}' of user '{1}' of chat '{2}'",
                    message.MessageId, message.From.Id, message.Chat.Id);
            }

            var spammerModel = new SpammerModel
            {
                ChannelId = channel.Id,
                TelegramId = message.From!.Id,
                FirstName = message.From.FirstName,
                LastName = message.From.LastName,
                UserName = message.From.Username
            };
            var spammer = await _spamService.UpdateOrCreateSpammerAsync(spammerModel);
            try
            {
                await _spamService.CreateAsync(new SpamMessage
                {
                    SpammerId = spammer.Id,
                    TelegramId = message.MessageId,
                    Value = text,
                    DateTimeWritten = DateTime.UtcNow,
                    Type = spamType,
                    Context = spamContext
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to add spam message '{0}' of user '{1}' of chat '{2}'",
                    message.MessageId, message.From.Id, message.Chat.Id);
            }

            var shouldBann = await _spamService.BannSpammerIfOutOfLimitAsync(spammer, Constants.SpamLimit);
            if (shouldBann)
            {
                try
                {
                    await _telegramBotClient.BanChatMemberAsync(message.Chat.Id, message.From.Id);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Failed to ban chat '{0}' user '{1}'", message.Chat.Id, message.From.Id);
                }
            }
        }
    }
}