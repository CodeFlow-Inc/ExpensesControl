using ExpensesControl.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpensesControl.Infrastructure.SqlServer.Persistence;

public class SqlContext(DbContextOptions<SqlContext> opts) : IdentityDbContext<IdentityUser, IdentityRole, string>(opts)
{
    public DbSet<Expense> Expense { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
