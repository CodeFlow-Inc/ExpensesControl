namespace ExpensesControl.Infrastructure.SqlServer.Repositories.Interface
{
    public interface IUnitOfWork
    {
        IExpenseRepository ExpenseRepository { get; }
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        void Dispose();
        Task RollbackAsync();
    }
}