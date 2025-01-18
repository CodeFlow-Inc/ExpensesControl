using ExpensesControl.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpensesControl.Infrastructure.Persistence
{
    public class SqlContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public SqlContext(DbContextOptions<SqlContext> opts) : base(opts) { }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<Despesa> Despesaes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
