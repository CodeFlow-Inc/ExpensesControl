using CodeFlow.Start.Lib.Context.Base.Repositories;
using ExpensesControl.Domain.Entities.AggregateRoot;
using ExpensesControl.Infrastructure.SqlServer.Persistence;
using ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;
using Microsoft.Extensions.Logging;

namespace ExpensesControl.Infrastructure.SqlServer.Repositories;

/// <summary>
/// Initializes a new instance of the ExpenseRepository class.
/// </summary>
/// <param name="context">The application database context.</param>
/// <param name="logger">The logger instance.</param>
public class ExpenseRepository(SqlContext context, ILogger<ExpenseRepository> logger) : BaseRepository<Expense, int>(context, logger), IExpenseRepository
{
}
