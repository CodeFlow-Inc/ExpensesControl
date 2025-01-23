using ExpensesControl.Application.UseCases;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesControl.Application.Config;

/// <summary>
/// Configuration class for setting up MediatR and related pipeline behaviors.
/// </summary>
public static class MediatorConfig
{
    /// <summary>
    /// Adds MediatR services and pipeline behaviors to the service collection.
    /// </summary>
    /// <param name="services">The service collection to which the services will be added.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(MediatorConfig).Assembly);
        });

        // Register the CommandFailureBehavior as a pipeline behavior for MediatR.
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandFailureBehavior<,>));

        return services;
    }
}
