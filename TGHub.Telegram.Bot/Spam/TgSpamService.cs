using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using TGHub.Application;
using TGHub.Application.Services.Channels;
using TGHub.Application.Services.Spammers;
using TGHub.Application.Services.Spammers.Models;
using TGHub.Application.Services.SpamMessages;
using TGHub.Application.Services.SpamWords;
using TGHub.Domain.Entities;
using TGHub.Domain.Enums;
using TGHub.SpamModeration;

namespace TGHub.Telegram.Bot.Spam;

public class TgSpamService : ITgSpamService
{
    private readonly IChannelService _channelService;
    private readonly ILogger<TgSpamService> _logger;
    private readonly ISpammerService _spammerService;
    private readonly ISpamMessageService _spamMessageService;
    private readonly ISpamModerator _spamModerator;
    private readonly ISpamWordsService _spamWordsService;
    private readonly ITelegramBotClient _telegramBotClient;

    public TgSpamService(IChannelService channelService, ISpamModerator spamModerator,
        ITelegramBotClient telegramBotClient, ISpamMessageService spamMessageService,
        ISpammerService spammerService, ISpamWordsService spamWordsService, ILogger<TgSpamService> logger)
    {
        _channelService = channelService;
        _spamModerator = spamModerator;
        _telegramBotClient = telegramBotClient;
        _spamMessageService = spamMessageService;
        _spammerService = spammerService;
        _spamWordsService = spamWordsService;
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
            var spamWordsFound = await _spamWordsService.FindSpamWordsAsync(text, channel.Id);
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
            var spammer = await _spammerService.UpdateOrCreateSpammerAsync(spammerModel);
            try
            {
                await _spamMessageService.CreateAsync(new SpamMessage
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

            var shouldBann = await _spammerService.BannSpammerIfOutOfLimitAsync(spammer, Constants.SpamLimit);
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