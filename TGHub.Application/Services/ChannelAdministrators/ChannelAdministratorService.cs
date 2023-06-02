using Microsoft.EntityFrameworkCore;
using TGHub.Application.Common.Filtering;
using TGHub.Application.Interfaces;
using TGHub.Application.Services.Base;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.ChannelAdministrators;

public class ChannelAdministratorService : Service<ChannelAdministrator>
{
    public ChannelAdministratorService(ITgHubDbContext dbContext) : base(dbContext)
    {
    }

    public override Task<List<ChannelAdministrator>> ListAsync(FilterBase<ChannelAdministrator>? filter = null)
    {
        var query = DbContext.ChannelAdministrators
            .Include(cha => cha.Channel)
            .Include(cha => cha.Administrator)
            .AsNoTracking();

        if (filter == null)
        {
            return query.ToListAsync();
        }

        if (filter.Where != null)
        {
            query = query.Where(filter.Where);
        }

        return query.Sort(filter).ToListAsync();
    }
}