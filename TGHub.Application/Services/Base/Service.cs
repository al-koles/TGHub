using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TGHub.Application.Common.Filtering;
using TGHub.Application.Interfaces;
using TGHub.Domain.Interfaces;

namespace TGHub.Application.Services.Base;

public class Service<TEntity> : IService<TEntity> where TEntity : class, IEntity
{
    protected readonly ITgHubDbContext DbContext;

    protected Service(ITgHubDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public Task<List<TEntity>> ListAsync(FilterBase<TEntity>? filter = null)
    {
        var query = DbContext.Set<TEntity>().AsNoTracking();

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

    public Task<TEntity?> FirsOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var query = DbContext.Set<TEntity>().AsNoTracking();

        if (predicate == null)
        {
            return query.FirstOrDefaultAsync();
        }

        return query.FirstOrDefaultAsync(predicate);
    }

    public async Task<int> CreateAsync(TEntity entity)
    {
        var entry = await DbContext.Set<TEntity>().AddAsync(entity);
        await DbContext.SaveChangesAsync();
        
        return entry.Entity.Id;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
        await DbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
        await DbContext.SaveChangesAsync();
    }
}