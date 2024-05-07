using Client.Persistence.Repositories;
using Client.Persistence.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Client.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddSingleton<IUnitOfWork, UnitOfWork>()
                .AddTransient<IServerService, ServerService>()
                .AddHttpClient<IServerService, ServerService>(opt => opt.BaseAddress = new Uri("https://localhost:7267/api"));
        return services;
    }
}
