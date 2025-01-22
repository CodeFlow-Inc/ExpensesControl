using ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;

namespace ExpensesControl.Infrastructure.SqlServer.Repositories.Base;

public interface IBaseRepository<T, TKey>
    where T : class
    where TKey : struct
{
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(T entity, TKey id, CancellationToken cancellationToken = default);
    T GetById(TKey id, CancellationToken cancellationToken = default);
    Task<T> GetByIdAsync(TKey id, Func<IQueryable<T>, IQueryable<T>>? include = null, CancellationToken cancellationToken = default);
    Task<T?> GetSingleBySpecificationAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
    IQueryable<T> ListAll();
    IQueryable<T> ListBySpecification(ISpecification<T> specification);
    Task<List<T>> ListBySpecificationAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
    Task<int> UpdateAsync(T entity, TKey id, CancellationToken cancellationToken = default);
}