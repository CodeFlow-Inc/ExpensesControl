using Ardalis.Specification;

namespace ExpensesControl.Infrastructure.SqlServer.Repositories.Base;

public interface IBaseRepository<TEntity, TKey>
    where TEntity : class
    where TKey : struct
{
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(TEntity entity, TKey id, CancellationToken cancellationToken = default);
    TEntity GetById(TKey id, CancellationToken cancellationToken = default);
    Task<TEntity> GetByIdAsync(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, CancellationToken cancellationToken = default);
    Task<TEntity?> GetSingleBySpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> ListBySpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task<int> UpdateAsync(TEntity entity, TKey id, CancellationToken cancellationToken = default);
}