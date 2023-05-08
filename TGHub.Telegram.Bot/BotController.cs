using Microsoft.AspNetCore.Mvc;
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
    private readonly IService<Channel> _channelService;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IService<TgHubUser> _userService;

    public BotController(ITelegramBotClient telegramBotClient, IService<Channel> channelService,
        IService<TgHubUser> userService)
    {
        _telegramBotClient = telegramBotClient;
        _channelService = channelService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update? update)
    {
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
        var dbChannel = await _channelService.FirsOrDefaultAsync(ch => ch.TelegramId == channelTelegramId);
        if (dbChannel != null)
        {
            dbChannel.IsActive = false;
            await _channelService.UpdateAsync(dbChannel);
        }
    }

    private async Task CreateOrUpdateChannelAsync(Chat tgChannel)
    {
        var dbChannel = await _channelService.FirsOrDefaultAsync(ch => ch.TelegramId == tgChannel.Id);
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
        var administrators = await TryGetChatAdministratorsAsync(tgChannel);
        List<TgHubUser> dbUsers = new();
        if (administrators.Any())
        {
            dbUsers = await _userService.ListAsync(new FilterBase<TgHubUser>
            {
                Where = u => administrators.Select(a => a.User.Id).Contains(u.TelegramId)
            });
        }

        dbChannel.TelegramId = tgChannel.Id;
        dbChannel.LinkedChatTelegramId = tgChannel.LinkedChatId;
        dbChannel.Name = tgChannel.Title ?? Guid.NewGuid().ToString();
        dbChannel.Administrators = administrators.Select(a => new ChannelAdministrator
        {
            Role = a.Status == ChatMemberStatus.Creator
                ? ChannelRole.Owner
                : ChannelRole.Administrator,
            Administrator = dbUsers.FirstOrDefault(u => u.TelegramId == a.User.Id) ?? new TgHubUser
            {
                TelegramId = a.User.Id,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                UserName = a.User.Username
            }
        }).ToList();
    }

    private async Task<ChatMember[]> TryGetChatAdministratorsAsync(Chat chat)
    {
        ChatMember[] administrators;
        try
        {
            administrators = await _telegramBotClient.GetChatAdministratorsAsync(chat.Id);
            administrators = administrators.Where(a => !a.User.IsBot).ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            administrators = new ChatMember[] { };
        }

        return administrators;
    }
}