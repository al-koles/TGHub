﻿using Microsoft.EntityFrameworkCore;
using TGHub.Application.Interfaces;
using TGHub.Application.Services.Base;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.SpamMessages;

public class SpamMessageService : Service<SpamMessage>, ISpamMessageService
{
    public SpamMessageService(ITgHubDbContext dbContext) : base(dbContext)
    {
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