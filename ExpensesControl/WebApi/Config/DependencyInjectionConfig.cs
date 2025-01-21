using ExpensesControl.Application.Config;
using ExpensesControl.Infrastructure.SqlServer.Ioc;

namespace ExpensesControl.WebApi.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services
            .ConfigureRepositoryIoc()
            .AddMediatR();
        return services;
    }
}