﻿namespace ExpensesControl.Infrastructure.Repositories.Interface
{
    public interface IBaseRepository<T, TKey>
        where T : class
        where TKey : struct
    {
        Task<T> CreateAsync(T entity);
        Task<bool> DeleteAsync(T entity, TKey id);
        T GetById(TKey id);
        Task<T> GetByIdAsync(TKey id, Func<IQueryable<T>, IQueryable<T>>? include = null);
        IQueryable<T> ListAll();
        Task<int> UpdateAsync(T entity, TKey id);
    }
}