using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TGHub.Application.Interfaces;
using TGHub.Application.Services.Base;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.ChannelAdministrators;

public class ChannelService : Service<Channel>
{
    public ChannelService(ITgHubDbContext dbContext) : base(dbContext)
    {
    }

    public override Task<Channel?> FirstOrDefaultAsync(Expression<Func<Channel, bool>>? predicate = null)
    {
        var query = DbContext.Channels
            .Include(ch => ch.Administrators)
            .ThenInclude(a => a.Administrator)
            .AsNoTracking();
        return predicate == null ? query.FirstOrDefaultAsync() : query.FirstOrDefaultAsync(predicate);
    }
}