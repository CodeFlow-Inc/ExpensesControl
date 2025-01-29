using CodeFlow.Data.Context.Package.Base.Repositories;
using ExpensesControl.Domain.Entities.AggregateRoot;
using ExpensesControl.Infrastructure.SqlServer.Persistence;
using ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;
using Microsoft.Extensions.Logging;

namespace ExpensesControl.Infrastructure.SqlServer.Repositories;

/// <summary>
/// Initializes a new instance of the IncomeRepository class.
/// </summary>
/// <param name="context">The application database context.</param>
/// <param name="logger">The logger instance.</param>
public class RevenueRepository(SqlContext context, ILogger<RevenueRepository> logger) : BaseRepository<Revenue, int>(context, logger), IRevenueRepository
{
}
