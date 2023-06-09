using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TGHub.Application.Common.Filtering;
using TGHub.Application.Interfaces;
using TGHub.Application.Services.Base;
using TGHub.Application.Services.Posts.Data;
using TGHub.Application.Services.Posts.Interfaces;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Posts;

public class PostService : Service<Post>, IPostService
{
    public PostService(ITgHubDbContext dbContext) : base(dbContext)
    {
    }

    public override Task<List<Post>> ListAsync(FilterBase<Post>? filter = null)
    {
        var query = DbContext.Posts
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

        if (filter is PostFilter postFilter)
        {
            if (postFilter.From.HasValue)
            {
                query = query.Where(p => p.ReleaseDateTime >= postFilter.From);
            }

            if (postFilter.To.HasValue)
            {
                query = query.Where(p => p.ReleaseDateTime <= postFilter.To);
            }

            if (postFilter.ChannelId.HasValue)
            {
                query = query.Where(p => p.Creator.ChannelId == postFilter.ChannelId);
            }

            if (postFilter.ChannelAdministratorId.HasValue)
            {
                query = query.Where(p =>
                    p.Creator.Channel.IsActive &&
                    p.Creator.Channel.Administrators.Any(a => a.AdministratorId == postFilter.ChannelAdministratorId &&
                                                              a.IsActive));
            }
        }

        return query.Sort(filter).ToListAsync();
    }

    public override Task<Post?> FirstOrDefaultAsync(Expression<Func<Post, bool>>? predicate = null)
    {
        var query = DbContext.Posts
            .Include(p => p.Attachments)
            .Include(p => p.Buttons)
            .Include(p => p.Creator)
            .ThenInclude(c => c.Administrator)
            .Include(p => p.Creator)
            .ThenInclude(p => p.Channel);

        return predicate == null
            ? query.FirstOrDefaultAsync()
            : query.FirstOrDefaultAsync(predicate);
    }
}