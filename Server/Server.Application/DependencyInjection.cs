using Microsoft.Extensions.DependencyInjection;

namespace Server.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(conf => conf.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));
        return services;
    }
}
