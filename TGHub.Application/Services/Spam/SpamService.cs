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

    public async Task<Spammer> UpdateOrCreateSpammerAsync(SpammerModel spammerModel)
    {
        var spammer = await DbContext.Spammers
            .FirstOrDefaultAsync(u => u.TelegramId == spammerModel.TelegramId &&
                                      u.ChannelId == spammerModel.ChannelId);
        if (spammer == null)
        {
            spammer = _mapper.Map<Spammer>(spammerModel);
            await DbContext.Spammers.AddAsync(spammer);
        }
        else
        {
            _mapper.Map(spammerModel, spammer);
        }

        await DbContext.SaveChangesAsync();

        return spammer;
    }

    public async Task<bool> BannSpammerIfOutOfLimitAsync(Spammer spammer, int spamMessagesLimit = 5)
    {
        DbContext.ChangeTracker.Clear();
        bool banned;

        var spammerIsAlreadyBanned = spammer.BannDateTime != null;
        if (spammerIsAlreadyBanned)
        {
            banned = true;
        }
        else
        {
            var lastSpamMessages = await GetLastSpamMessagesBySpammerAsync(spammer);

            var userIsOutOfSpamMessagesLimit = lastSpamMessages.Count > spamMessagesLimit;
            if (userIsOutOfSpamMessagesLimit)
            {
                spammer.BannDateTime = DateTime.UtcNow;
                spammer.BannContext = string.Join($"{Environment.NewLine}{Environment.NewLine}",
                    lastSpamMessages.Select(m => m.Value));
                banned = true;
            }
            else
            {
                banned = false;
            }
        }

        await DbContext.SaveChangesAsync();
        return banned;
    }

    public async Task<List<SpamMessage>> GetLastSpamMessagesBySpammerAsync(Spammer spammer)
    {
        var spamMessages = DbContext.SpamMessages
            .Where(m => m.SpammerId == spammer.Id);

        var spammerIsBanned = spammer.BannDateTime != null;
        if (spammerIsBanned)
        {
            spamMessages = spamMessages.Where(m => m.DateTimeWritten > spammer.BannDateTime);
        }
        else
        {
            var lastArchiveBann = await DbContext.ArchiveBanns.OrderBy(b => b.From)
                .LastOrDefaultAsync(b => b.SpammerId == spammer.Id);
            if (lastArchiveBann != null)
            {
                spamMessages = spamMessages.Where(m => m.DateTimeWritten > lastArchiveBann.From);
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