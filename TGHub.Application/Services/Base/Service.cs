﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TGHub.Application.Common.Filtering;
using TGHub.Application.Interfaces;
using TGHub.Domain.Interfaces;

namespace TGHub.Application.Services.Base;

public class Service<TEntity> : IService<TEntity> where TEntity : class, IEntity
{
    protected readonly ITgHubDbContext DbContext;

    public Service(ITgHubDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public virtual Task<List<TEntity>> ListAsync(FilterBase<TEntity>? filter = null)
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

    public virtual Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var query = DbContext.Set<TEntity>();

        return predicate == null
            ? query.FirstOrDefaultAsync()
            : query.FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<int> CreateAsync(TEntity entity)
    {
        var entry = await DbContext.Set<TEntity>().AddAsync(entity);
        await DbContext.SaveChangesAsync();

        return entry.Entity.Id;
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
        await DbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
        await DbContext.SaveChangesAsync();
    }
}