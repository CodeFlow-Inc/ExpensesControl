using CodeFlow.Data.Context.Package.Base.Context;
using ExpensesControl.Domain.Entities.AggregateRoot;
using Microsoft.EntityFrameworkCore;

namespace ExpensesControl.Infrastructure.SqlServer.Persistence;

/// <summary>
/// Represents the context of the SQL database.
/// </summary>
/// <param name="opts"></param>
public class SqlContext(DbContextOptions<SqlContext> opts) : BaseDbContext(opts)
{
	public DbSet<Expense> Expense { get; set; }
	public DbSet<Revenue> Revenue { get; set; }

	/// <summary>
	/// Configures the model of the database.
	/// </summary>
	/// <param name="modelBuilder"></param>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlContext).Assembly);
	}
}
