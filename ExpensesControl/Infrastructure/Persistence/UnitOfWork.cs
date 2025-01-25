using ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace ExpensesControl.Infrastructure.SqlServer.Persistence;

/// <summary>
/// Implements the Unit of Work pattern to manage transactions and database changes.
/// </summary>
public class UnitOfWork(SqlContext context, IExpenseRepository expenseRepository, IRevenueRepository revenueRepository) : IUnitOfWork, IDisposable
{
    public IExpenseRepository ExpenseRepository { get; } = expenseRepository;
    public IRevenueRepository RevenueRepository { get; } = revenueRepository;
    private IDbContextTransaction? _currentTransaction;
    private bool _disposed;

    /// <summary>
    /// Starts a new transaction in the database.
    /// </summary>
    /// <param name="cancellationToken">A token for observing cancellation.</param>
    /// <exception cref="InvalidOperationException">Thrown if a transaction is already in progress.</exception>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
            throw new InvalidOperationException("Uma transação já está em andamento.");

        _currentTransaction = await context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
    }

    /// <summary>
    /// Commits the current transaction and saves all changes made in the context.
    /// </summary>
    /// <param name="cancellationToken">A token for observing cancellation.</param>
    /// <returns>The number of state entries written to the database.</returns>
    /// <exception cref="InvalidOperationException">Thrown if there is no transaction in progress.</exception>
    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
            throw new InvalidOperationException("Nenhuma transação está em andamento. Chame BeginTransactionAsync primeiro.");

        try
        {
            int result = await context.SaveChangesAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if there is no transaction in progress.</exception>
    public async Task RollbackAsync()
    {
        if (_currentTransaction == null)
            throw new InvalidOperationException("Nenhuma transação está em andamento para ser revertida.");

        await _currentTransaction.RollbackAsync();
        await DisposeTransactionAsync();
    }

    /// <summary>
    /// Releases the current transaction, if it exists.
    /// </summary>
    private async Task DisposeTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    /// <summary>
    /// Releases unmanaged resources and optionally managed resources.
    /// </summary>
    /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                context.Dispose();
                _currentTransaction?.Dispose();
            }
            _disposed = true;
        }
    }

    /// <summary>
    /// Releases all resources used by the UnitOfWork instance.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
