using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TGHub.Application.Interfaces;
using TGHub.Application.Services.Base;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Channels;

public class ChannelService : Service<Channel>, IChannelService
{
    public ChannelService(ITgHubDbContext dbContext) : base(dbContext)
    {
    }

    public override Task<Channel?> FirstOrDefaultAsync(Expression<Func<Channel, bool>>? predicate = null)
    {
        var query = DbContext.Channels
            .Include(ch => ch.Administrators)
            .ThenInclude(a => a.Administrator);
        return predicate == null ? query.FirstOrDefaultAsync() : query.FirstOrDefaultAsync(predicate);
    }

    public async Task MarkChannelInactiveIfExistAsync(long channelTelegramId)
    {
        var dbChannel = await FirstOrDefaultAsync(ch => ch.TelegramId == channelTelegramId);
        if (dbChannel != null)
        {
            dbChannel.IsActive = false;
            await UpdateAsync(dbChannel);
        }
    }

    public Task<List<Spammer>> GetBannedUsersAsync(int channelId)
    {
        return DbContext.Spammers
            .Where(u => u.ChannelId == channelId &&
                        u.BannDateTime != null)
            .OrderByDescending(u => u.BannDateTime)
            .ToListAsync();
    }
}