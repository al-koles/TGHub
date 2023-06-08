using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TGHub.Application.Services.Channels;
using TGHub.Application.Services.Lotteries.Interfaces;
using TGHub.Domain.Entities;
using TGHub.Telegram.Bot.Channels;
using TGHub.Telegram.Bot.Spam;

namespace TGHub.Telegram.Bot;

[Route("api/[controller]")]
[ApiController]
public class BotController : ControllerBase
{
    private readonly IChannelService _channelService;
    private readonly ILogger<BotController> _logger;
    private readonly ILotteryService _lotteryService;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ITgChannelService _tgChannelService;
    private readonly ITgSpamService _tgSpamService;

    public BotController(ITelegramBotClient telegramBotClient, IChannelService channelService,
        ILogger<BotController> logger, ITgChannelService tgChannelService, ILotteryService lotteryService,
        ITgSpamService tgSpamService)
    {
        _telegramBotClient = telegramBotClient;
        _channelService = channelService;
        _logger = logger;
        _tgChannelService = tgChannelService;
        _lotteryService = lotteryService;
        _tgSpamService = tgSpamService;
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

        Channel? channel = null;
        switch (message.Chat.Type)
        {
            case ChatType.Channel:
                channel = await _channelService.FirstOrDefaultAsync(ch => ch.TelegramId == message.Chat.Id);
                if (channel == null)
                {
                    await _tgChannelService.CreateOrUpdateChannelFromTgAsync(message.Chat.Id);
                    channel = await _channelService.FirstOrDefaultAsync(ch => ch.TelegramId == message.Chat.Id);
                }

                break;
            case ChatType.Supergroup:
                channel = await _channelService.FirstOrDefaultAsync(ch => ch.LinkedChatTelegramId == message.Chat.Id);
                if (channel == null)
                {
                    var tgSupergroup = await _telegramBotClient.GetChatAsync(message.Chat.Id);
                    if (tgSupergroup.LinkedChatId != null)
                    {
                        channel = await _channelService
                            .FirstOrDefaultAsync(ch => ch.TelegramId == tgSupergroup.LinkedChatId);
                        if (channel == null)
                        {
                            await _tgChannelService.CreateOrUpdateChannelFromTgAsync(tgSupergroup.LinkedChatId.Value);
                            channel = await _channelService
                                .FirstOrDefaultAsync(ch => ch.TelegramId == tgSupergroup.LinkedChatId);
                        }
                        else
                        {
                            channel.LinkedChatTelegramId = tgSupergroup.Id;
                            channel.IsActive = true;
                            await _channelService.UpdateAsync(channel);
                        }
                    }
                }

                break;
        }

        if (channel != null)
        {
            string messageText = null!;
            if (!string.IsNullOrEmpty(message.Text))
            {
                messageText = message.Text;
            }
            else if (!string.IsNullOrEmpty(message.Caption))
            {
                messageText = message.Caption;
            }

            if (!string.IsNullOrEmpty(messageText))
            {
                await _tgSpamService.CheckMessageForSpamAsync(message, messageText);
            }
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
        var user = update.CallbackQuery!.From;
        if (int.TryParse(update.CallbackQuery?.Data, out var lotteryId))
        {
            if (user.Username == null)
            {
                await _telegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id,
                    "Go to Telegram setting and specify your account username before", true);
                return;
            }

            var lottery = await _lotteryService.FirstOrDefaultAsync(l => l.Id == lotteryId);
            if (lottery == null)
            {
                await _telegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery!.Id,
                    "This lottery was deleted");
                return;
            }

            if (lottery.ResultTelegramId != null)
            {
                await _telegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery!.Id,
                    "This lottery ended");
                return;
            }

            var participant = lottery.Participants
                .FirstOrDefault(p => p.TelegramId == user.Id);
            if (participant == null)
            {
                lottery.Participants.Add(new LotteryParticipant
                {
                    TelegramId = user.Id,
                    NickName = user.Username
                });
                await _lotteryService.UpdateAsync(lottery);
                await _telegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id,
                    "Congratulations! You are a participant now", true);
            }
            else
            {
                await _telegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id,
                    "You are already a participant");
            }
        }
        else
        {
            _logger.LogError(
                $"Can't parse CallbackQuery date ({update.CallbackQuery?.Data}) " +
                $"sent by user ({user.Id}) " +
                $"in chat ({update.CallbackQuery!.ChatInstance})");
            await _telegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery!.Id,
                "Error. We've logged it and it will be fixed soon", true);
        }
    }
}