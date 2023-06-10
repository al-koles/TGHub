using Microsoft.EntityFrameworkCore;
using TGHub.Application.Common.Filtering;
using TGHub.Application.Interfaces;
using TGHub.Application.Services.Base;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.ArchiveBanns;

public class ArchiveBannService : Service<ArchiveBann>, IArchiveBannService
{
    public ArchiveBannService(ITgHubDbContext dbContext) : base(dbContext)
    {
    }

    public override Task<List<ArchiveBann>> ListAsync(FilterBase<ArchiveBann>? filter = null)
    {
        var query = DbContext.ArchiveBanns
            .Include(b => b.Spammer)
            .Include(b => b.Initiator)
            .ThenInclude(i => i == null ? null : i.Administrator)
            .Include(b => b.UnBannInitiator)
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

        query = query.Sort(filter);

        if (filter.Skip.HasValue)
        {
            query = query.Skip(filter.Skip.Value);
        }

        if (filter.Take.HasValue)
        {
            query = query.Take(filter.Take.Value);
        }

        return query.ToListAsync();
    }
}