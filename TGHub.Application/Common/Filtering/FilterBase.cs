using System.Linq.Expressions;
using TGHub.Domain.Interfaces;

namespace TGHub.Application.Common.Filtering;

public class FilterBase<TEntity> where TEntity : class, IEntity
{
    public Expression<Func<TEntity, bool>>? Where { get; set; }
    public int? Take { get; set; }
    public int? Skip { get; set; }
    public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
    public Expression<Func<TEntity, dynamic>> SortBy { get; set; } = e => e.Id;
}