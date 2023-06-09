using System.Globalization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TGHub.Application.Common.Filtering;
using TGHub.Application.Interfaces;
using TGHub.Application.Services.Base;
using TGHub.Application.Services.Spammers.Models;
using TGHub.Application.Services.SpamMessages;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Spammers;

public class SpammerService : Service<Spammer>, ISpammerService
{
    private readonly IMapper _mapper;
    private readonly ISpamMessageService _spamMessageService;

    public SpammerService(ITgHubDbContext dbContext, ISpamMessageService spamMessageService,
        IMapper mapper) : base(dbContext)
    {
        _spamMessageService = spamMessageService;
        _mapper = mapper;
    }

    public override Task<List<Spammer>> ListAsync(FilterBase<Spammer>? filter = null)
    {
        var query = DbContext.Spammers
            .Include(s => s.Channel)
            .Include(s => s.BannInitiator)
            .ThenInclude(i => i == null ? null : i.Administrator)
            .AsNoTracking();

        if (filter == null)
        {
            return query.ToListAsync();
        }

        if (filter.Where != null)
        {
            query = query.Where(filter.Where);
        }

        if (filter.Skip.HasValue)
        {
            query = query.Skip(filter.Skip.Value);
        }

        if (filter.Take.HasValue)
        {
            query = query.Take(filter.Take.Value);
        }

        return query.Sort(filter).ToListAsync();
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
            var lastSpamMessages = await _spamMessageService.GetLastSpamMessagesBySpammerAsync(spammer);

            var userIsOutOfSpamMessagesLimit = lastSpamMessages.Count > spamMessagesLimit;
            if (userIsOutOfSpamMessagesLimit)
            {
                spammer.BannDateTime = DateTime.UtcNow;
                spammer.BannContext = string.Join($"{Environment.NewLine}",
                    lastSpamMessages.Select(m =>
                        $"{m.DateTimeWritten.ToString(CultureInfo.InvariantCulture.DateTimeFormat.SortableDateTimePattern)} {m.Value}"));
                DbContext.ChangeTracker.Clear();
                await UpdateAsync(spammer);
                banned = true;
            }
            else
            {
                banned = false;
            }
        }

        return banned;
    }
}