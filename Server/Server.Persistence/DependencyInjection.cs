using Microsoft.Extensions.DependencyInjection;
using Server.Persistence.Repositories.FakeRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
