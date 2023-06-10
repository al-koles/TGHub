using Microsoft.EntityFrameworkCore;
using TGHub.Application.Interfaces;
using TGHub.Application.Services.Base;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.SpamWords;

public class SpamWordsService : Service<SpamWord>, ISpamWordsService
{
    public SpamWordsService(ITgHubDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<string>> FindSpamWordsAsync(string text, int channelId)
    {
        var spamWords = await DbContext.SpamWords.Where(w => w.ChannelId == channelId).ToListAsync();
        var detectedSpamWords = spamWords.Where(w => text.ToLower().Contains(w.Value.ToLower()))
            .Select(w => w.Value).ToList();

        return detectedSpamWords;
    }
}