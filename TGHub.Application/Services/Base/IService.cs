using System.Linq.Expressions;
using TGHub.Application.Common.Filtering;
using TGHub.Domain.Interfaces;

namespace TGHub.Application.Services.Base;

public interface IService<TEntity> where TEntity : class, IEntity
{
    Task<List<TEntity>> ListAsync(FilterBase<TEntity>? filter = null);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null);
    Task<int> CreateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}