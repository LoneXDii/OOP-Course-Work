using Microsoft.Extensions.DependencyInjection;
using Server.Persistence.Repositories.FakeRepositories;

namespace Server.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddSingleton<IUnitOfWork, FakeUnitOfWork>();
        return services;
    }

    //DB DI
    //public static IServiceCollection AddPersistence(this IServiceCollection services, DbContextOptions options)
    //{
    //    services.AddPersistence()
    //            .AddSingleton<AppDbContext>(new AppDbContext((DbContextOptions<AppDbContext>)options));
    //    return services;
    //}
}
