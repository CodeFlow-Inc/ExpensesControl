using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesControl.Application.Config;

/// <summary>
/// Provides configuration for FluentValidation in the application.
/// </summary>
public static class ValidatorConfig
{
	/// <summary>
	/// Registers all validators found in the assembly containing the ValidatorConfig class into the dependency injection container.
	/// </summary>
	/// <param name="services">The service collection to add the validators to.</param>
	/// <returns>The updated service collection with validators registered.</returns>
	public static IServiceCollection ConfigureValidator(this IServiceCollection services)
	{
		AssemblyScanner.FindValidatorsInAssembly(typeof(ValidatorConfig).Assembly)
			.ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));
		return services;
	}
}
