using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TGHub.Application.Common.Filtering;
using TGHub.Application.Services.Base;
using TGHub.Domain.Entities;
using TGHub.Domain.Enums;

namespace TGHub.Telegram.Bot;

[Route("api/[controller]")]
[ApiController]
public class BotController : ControllerBase
{
    private readonly IService<ChannelAdministrator> _channelAdministratorService;
    private readonly IService<Channel> _channelService;
    private readonly ILogger<BotController> _logger;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IService<TgHubUser> _userService;

    public BotController(ITelegramBotClient telegramBotClient, IService<Channel> channelService,
        IService<TgHubUser> userService, IService<ChannelAdministrator> channelAdministratorService,
        ILogger<BotController> logger)
    {
        _telegramBotClient = telegramBotClient;
        _channelService = channelService;
        _userService = userService;
        _channelAdministratorService = channelAdministratorService;
        _logger = logger;
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
                    await ChatMember(update);
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

    private async Task ChatMember(Update update)
    {
        var member = update.MyChatMember;
        if (member == null)
        {
            return;
        }

        try
        {
            if (member.NewChatMember.Status == ChatMemberStatus.Administrator)
            {
                if (member.Chat.Type == ChatType.Channel)
                {
                    await CreateOrUpdateChannelAsync(member.Chat);
                }
            }
            else
            {
                if (member.Chat.Type == ChatType.Channel)
                {
                    await MarkChannelInactiveIfExist(member.Chat.Id);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task MarkChannelInactiveIfExist(long channelTelegramId)
    {
        var dbChannel = await _channelService.FirstOrDefaultAsync(ch => ch.TelegramId == channelTelegramId);
        if (dbChannel != null)
        {
            dbChannel.IsActive = false;
            await _channelService.UpdateAsync(dbChannel);
        }
    }

    private async Task CreateOrUpdateChannelAsync(Chat tgChannel)
    {
        var dbChannel = await _channelService.FirstOrDefaultAsync(ch => ch.TelegramId == tgChannel.Id);
        if (dbChannel == null)
        {
            await CreateChannelFromTgAsync(tgChannel);
        }
        else
        {
            await UpdateChannelFromTgAsync(tgChannel, dbChannel);
        }
    }

    private async Task CreateChannelFromTgAsync(Chat tgChannel)
    {
        var channel = new Channel();
        await FillFromTgChannelAsync(tgChannel, channel);
        await _channelService.CreateAsync(channel);
    }

    private async Task UpdateChannelFromTgAsync(Chat tgChannel, Channel dbChannel)
    {
        await FillFromTgChannelAsync(tgChannel, dbChannel);
        await _channelService.UpdateAsync(dbChannel);
    }

    private async Task FillFromTgChannelAsync(Chat tgChannel, Channel dbChannel)
    {
        dbChannel.TelegramId = tgChannel.Id;
        dbChannel.LinkedChatTelegramId = tgChannel.LinkedChatId;
        dbChannel.Name = tgChannel.Title ?? Guid.NewGuid().ToString();
        dbChannel.IsActive = true;

        try
        {
            var tgAdministrators = await FetchChatAdministratorsAsync(tgChannel);
            var administratorsToAdd = tgAdministrators.Where(tgAdmin =>
                    dbChannel.Administrators.All(dbAdmin =>
                        dbAdmin.Administrator.TelegramId != tgAdmin.User.Id))
                .ToList();

            List<TgHubUser> existingDbUsersToAdd = new();
            if (administratorsToAdd.Any())
            {
                existingDbUsersToAdd = await _userService.ListAsync(new FilterBase<TgHubUser>
                {
                    Where = u => administratorsToAdd.Select(a => a.User.Id).Contains(u.TelegramId)
                });
            }

            dbChannel.Administrators = dbChannel.Administrators
                .Concat(administratorsToAdd.Select(tgAdmin =>
                    new ChannelAdministrator
                    {
                        Role = tgAdmin.Status == ChatMemberStatus.Creator
                            ? ChannelRole.Owner
                            : ChannelRole.Administrator,
                        IsActive = true,
                        Administrator =
                            existingDbUsersToAdd.FirstOrDefault(dbUser => dbUser.TelegramId == tgAdmin.User.Id)
                            ?? new TgHubUser
                            {
                                TelegramId = tgAdmin.User.Id,
                                FirstName = tgAdmin.User.FirstName,
                                LastName = tgAdmin.User.LastName,
                                UserName = tgAdmin.User.Username
                            }
                    }))
                .ToList();
            
            foreach (var dbAdmin in dbChannel.Administrators)
            {
                dbAdmin.IsActive = tgAdministrators.Any(tgAdmin =>
                    tgAdmin.User.Id == dbAdmin.Administrator.TelegramId);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error fetching administrators of channel ({0})", tgChannel.Id);
        }
    }

    private async Task<ChatMember[]> FetchChatAdministratorsAsync(Chat chat)
    {
        var administrators = await _telegramBotClient.GetChatAdministratorsAsync(chat.Id);
        return administrators.Where(a => !a.User.IsBot).ToArray();
    }
}