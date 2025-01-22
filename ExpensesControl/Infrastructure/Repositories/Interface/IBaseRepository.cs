namespace ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;

public interface IBaseRepository<T, TKey>
    where T : class
    where TKey : struct
{
    Task<T> CreateAsync(T entity);
    Task<bool> DeleteAsync(T entity, TKey id);
    T GetById(TKey id);
    Task<T> GetByIdAsync(TKey id, Func<IQueryable<T>, IQueryable<T>>? include = null);
    Task<T?> GetSingleBySpecificationAsync(ISpecification<T> specification);
    IQueryable<T> ListAll();
    IQueryable<T> ListBySpecification(ISpecification<T> specification);
    Task<List<T>> ListBySpecificationAsync(ISpecification<T> specification);
    Task<int> UpdateAsync(T entity, TKey id);
    Task<int> SaveChangesAsync();
}