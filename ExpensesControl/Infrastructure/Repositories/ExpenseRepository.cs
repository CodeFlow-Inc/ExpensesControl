using ExpensesControl.Domain.Entities.AggregateRoot;
using ExpensesControl.Infrastructure.SqlServer.Persistence;
using ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;
using Microsoft.Extensions.Logging;

namespace ExpensesControl.Infrastructure.SqlServer.Repositories;

public class ExpenseRepository : BaseRepository<Expense, int>, IExpenseRepository
{
    /// <summary>
    /// Initializes a new instance of the ExpenseRepository class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="logger">The logger instance.</param>
    protected ExpenseRepository(SqlContext context, ILogger<ExpenseRepository> logger) : base(context, logger)
    {
    }
}
