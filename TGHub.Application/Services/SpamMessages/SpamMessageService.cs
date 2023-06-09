using Microsoft.EntityFrameworkCore;
using TGHub.Application.Common.Filtering;
using TGHub.Application.Interfaces;
using TGHub.Application.Services.Base;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.SpamMessages;

public class SpamMessageService : Service<SpamMessage>, ISpamMessageService
{
    public SpamMessageService(ITgHubDbContext dbContext) : base(dbContext)
    {
    }

    public override Task<List<SpamMessage>> ListAsync(FilterBase<SpamMessage>? filter = null)
    {
        var query = DbContext.SpamMessages
            .Include(m => m.Spammer)
            .ThenInclude(s => s.Channel)
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
}