using ExpensesControl.Domain.Entities.AggregateRoot;
using Microsoft.EntityFrameworkCore;

namespace ExpensesControl.Infrastructure.SqlServer.Persistence;

public class SqlContext(DbContextOptions<SqlContext> opts) : DbContext(opts)
{
    public DbSet<Expense> Expense { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlContext).Assembly);
    }
}
