using Microsoft.Extensions.DependencyInjection;

namespace ExpensesControl.Application.Config;

public static class MediatorConfig
{
    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(MediatorConfig).Assembly);
        });
        return services;
    }
}