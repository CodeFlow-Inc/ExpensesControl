using ExpensesControl.Application.Config;
using ExpensesControl.Infrastructure.SqlServer.Persistence;
using ExpensesControl.Infrastructure.SqlServer.Repositories;
using ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;

namespace ExpensesControl.WebApi.Config;

/// <summary>
/// Configures dependency injection for the application services.
/// </summary>
public static class DependencyInjectionConfig
{
	/// <summary>
	/// Adds dependency injection configuration to the service collection.
	/// </summary>
	/// <param name="services">The service collection to configure.</param>
	/// <returns>The configured service collection.</returns>
	public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
	{
		services
			.ConfigureRepositoryIoc()
			.ConfigureValidator()
			.AddMediatR();

		return services;
	}

	/// <summary>
	/// Configures IoC container for repository services.
	/// </summary>
	/// <param name="services">The service collection.</param>
	private static IServiceCollection ConfigureRepositoryIoc(this IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IExpenseRepository, ExpenseRepository>();
		services.AddScoped<IRevenueRepository, RevenueRepository>();
		return services;
	}

}
