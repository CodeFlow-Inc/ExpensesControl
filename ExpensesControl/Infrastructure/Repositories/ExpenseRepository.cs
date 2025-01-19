using ExpensesControl.Domain.Emtities;
using ExpensesControl.Infrastructure.Persistence;
using ExpensesControl.Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace ExpensesControl.Infrastructure.Repositories;

public class ExpenseRepository : IBaseRepository<Expense, int> 
{
    private readonly SqlContext _context;
    private readonly ILogger<ExpenseRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the ExpenseRepository class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="logger">The logger instance.</param>
    protected ExpenseRepository(SqlContext context, ILogger<BaseRepository<T, TKey>> logger) : base (context, _logger)
    {
    }
}
