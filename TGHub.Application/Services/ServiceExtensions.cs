using TGHub.Application.Common.Filtering;
using TGHub.Domain.Interfaces;

namespace TGHub.Application.Services;

public static class ServiceExtensions
{
    public static IQueryable<TEntity> Sort<TEntity>(this IQueryable<TEntity> entities, FilterBase<TEntity> filter)
        where TEntity : class, IEntity
    {
        return filter.SortDirection == SortDirection.Ascending
            ? entities.OrderBy(filter.SortBy)
            : entities.OrderByDescending(filter.SortBy);
    }
}