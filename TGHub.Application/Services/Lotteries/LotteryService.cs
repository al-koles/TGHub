using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TGHub.Application.Common.Filtering;
using TGHub.Application.Interfaces;
using TGHub.Application.Services.Base;
using TGHub.Application.Services.Lotteries.Data;
using TGHub.Application.Services.Lotteries.Interfaces;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Lotteries;

public class LotteryService : Service<Lottery>, ILotteryService
{
    public LotteryService(ITgHubDbContext dbContext) : base(dbContext)
    {
    }

    public override Task<List<Lottery>> ListAsync(FilterBase<Lottery>? filter = null)
    {
        var query = DbContext.Lotteries
            .AsNoTracking();

        if (filter == null)
        {
            return query.ToListAsync();
        }

        if (filter.Where != null)
        {
            query = query.Where(filter.Where);
        }

        if (filter is LotteryFilter lotteryFilter)
        {
            if (lotteryFilter.From.HasValue)
            {
                query = query.Where(l => l.StartDateTime >= lotteryFilter.From || l.EndDateTime >= lotteryFilter.From);
            }

            if (lotteryFilter.To.HasValue)
            {
                query = query.Where(l => l.StartDateTime <= lotteryFilter.To || l.EndDateTime <= lotteryFilter.To);
            }

            if (lotteryFilter.ChannelId.HasValue)
            {
                query = query.Where(l => l.Creator.ChannelId == lotteryFilter.ChannelId);
            }

            if (lotteryFilter.ChannelAdministratorId.HasValue)
            {
                query = query.Where(l =>
                    l.Creator.Channel.IsActive &&
                    l.Creator.Channel.Administrators.Any(a =>
                        a.AdministratorId == lotteryFilter.ChannelAdministratorId &&
                        a.IsActive));
            }
        }

        return query.Sort(filter).ToListAsync();
    }

    public override Task<Lottery?> FirstOrDefaultAsync(Expression<Func<Lottery, bool>>? predicate = null)
    {
        var query = DbContext.Lotteries
            .Include(l => l.Attachments)
            .Include(l => l.Participants)
            .Include(l => l.Creator)
            .ThenInclude(c => c.Administrator)
            .Include(l => l.Creator)
            .ThenInclude(l => l.Channel);

        return predicate == null
            ? query.FirstOrDefaultAsync()
            : query.FirstOrDefaultAsync(predicate);
    }
}