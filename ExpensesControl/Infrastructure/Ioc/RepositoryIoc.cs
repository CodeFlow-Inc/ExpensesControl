using ExpensesControl.Infrastructure.SqlServer.Persistence;
using ExpensesControl.Infrastructure.SqlServer.Repositories;
using ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesControl.Infrastructure.SqlServer.Ioc
{
    /// <summary>
    /// IoC configuration class for repository services.
    /// </summary>
    public static class RepositoryIoc
    {
        /// <summary>
        /// Configures IoC container for repository services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static IServiceCollection ConfigureRepositoryIoc(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IRevenueRepository, RevenueRepositoryy>();
            return services;
        }
    }
}
