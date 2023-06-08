using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TGHub.Application.Interfaces;
using TGHub.Application.Services.Base;
using TGHub.Application.Services.Spam.Models;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Spam;

public class SpamService : Service<SpamMessage>, ISpamService
{
    private readonly IMapper _mapper;

    public SpamService(ITgHubDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    public async Task<bool> BannSpammerIfOutOfLimitAsync(SpammerModel spammer, int spamMessagesLimit = 5)
    {
        bool banned;

        var bannUser = await DbContext.BannUsers
            .FirstOrDefaultAsync(u => u.TelegramId == spammer.TelegramId && u.ChannelId == spammer.ChannelId);
        if (bannUser == null)
        {
            await DbContext.BannUsers.AddAsync(_mapper.Map<BannUser>(spammer));
            banned = false;
        }
        else
        {
            _mapper.Map(spammer, bannUser);

            var userIsAlreadyBanned = bannUser.BannDateTime != null;
            if (userIsAlreadyBanned)
            {
                banned = true;
            }
            else
            {
                var lastSpamMessages = await GetLastSpamMessagesAsync(spammer.ChannelId, spammer.TelegramId);

                var userIsOutOfSpamMessagesLimit = lastSpamMessages.Count > spamMessagesLimit;
                if (userIsOutOfSpamMessagesLimit)
                {
                    bannUser.BannDateTime = DateTime.UtcNow;
                    bannUser.BannContext = string.Join($"{Environment.NewLine}{Environment.NewLine}",
                        lastSpamMessages.Select(m => m.Value));
                    banned = true;
                }
                else
                {
                    banned = false;
                }
            }
        }

        await DbContext.SaveChangesAsync();
        return banned;
    }

    public async Task<List<SpamMessage>> GetLastSpamMessagesAsync(int channelId, long userId)
    {
        var spamMessages = DbContext.SpamMessages.Where(m => m.ChannelId == channelId);

        var bannUser = await DbContext.BannUsers
            .FirstOrDefaultAsync(u => u.ChannelId == channelId && u.TelegramId == userId);
        if (bannUser != null)
        {
            if (bannUser.BannDateTime != null)
            {
                spamMessages = spamMessages.Where(m => m.DateTimeWritten > bannUser.BannDateTime);
            }
            else
            {
                var lastBann = await DbContext.Banns.OrderBy(b => b.From)
                    .LastOrDefaultAsync(b => b.UserId == userId);
                if (lastBann != null)
                {
                    spamMessages = spamMessages.Where(m => m.DateTimeWritten > lastBann.From);
                }
            }
        }

        return await spamMessages.OrderByDescending(m => m.DateTimeWritten).ToListAsync();
    }

    public async Task<List<string>> FindSpamWordsAsync(string text, int channelId)
    {
        var spamWords = await DbContext.SpamWords.Where(w => w.ChannelId == channelId).ToListAsync();
        var detectedSpamWords = spamWords.Where(w => text.ToLower().Contains(w.Value.ToLower()))
            .Select(w => w.Value).ToList();

        return detectedSpamWords;
    }
}