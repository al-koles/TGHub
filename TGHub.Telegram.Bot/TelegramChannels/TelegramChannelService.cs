using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TGHub.Application.Common.Exceptions;
using TGHub.Application.Common.Filtering;
using TGHub.Application.Services.Base;
using TGHub.Application.Services.Channels;
using TGHub.Domain.Entities;
using TGHub.Domain.Enums;

namespace TGHub.Telegram.Bot.TelegramChannels;

internal class TelegramChannelService : ITelegramChannelService
{
    private readonly IChannelService _channelService;
    private readonly ILogger<TelegramChannelService> _logger;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IService<TgHubUser> _userService;

    public TelegramChannelService(ITelegramBotClient telegramBotClient, IChannelService channelService,
        ILogger<TelegramChannelService> logger, IService<TgHubUser> userService)
    {
        _telegramBotClient = telegramBotClient;
        _channelService = channelService;
        _logger = logger;
        _userService = userService;
    }

    public async Task CreateOrUpdateChannelFromTgAsync(long channelTelegramId)
    {
        var tgChannel = await _telegramBotClient.GetChatAsync(channelTelegramId);
        if (tgChannel == null)
        {
            throw new NotFoundException("Telegram channel", channelTelegramId);
        }

        var dbChannel = await _channelService.FirstOrDefaultAsync(ch => ch.TelegramId == channelTelegramId);
        if (dbChannel == null)
        {
            await CreateChannelFromTgAsync(tgChannel);
        }
        else
        {
            await UpdateChannelFromTgAsync(tgChannel, dbChannel);
        }
    }

    public async Task UpdateCommentsGroupAsync(long commentsGroupTelegramId)
    {
        var tgCommentsGroup = await _telegramBotClient.GetChatAsync(commentsGroupTelegramId);
        if (tgCommentsGroup == null)
        {
            throw new NotFoundException("Telegram comments chat", commentsGroupTelegramId);
        }

        var channelTelegramId = tgCommentsGroup.LinkedChatId;
        if (channelTelegramId == null)
        {
            var channel = await _channelService
                .FirstOrDefaultAsync(ch => ch.LinkedChatTelegramId == commentsGroupTelegramId);
            if (channel != null)
            {
                channel.LinkedChatTelegramId = null;
                await _channelService.UpdateAsync(channel);
            }
        }
        else
        {
            await CreateOrUpdateChannelFromTgAsync(channelTelegramId.Value);
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