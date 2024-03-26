using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Application;
using Server.Persistence;
using Server.Services;

IHost host = Host.CreateDefaultBuilder().ConfigureServices(
    services =>
    {
        services.AddSingleton<IApplicationService, ApplicationService>()
                .AddPersistence()
                .AddApplication();
    }).Build();

var app = host.Services.GetRequiredService<IApplicationService>();

app.Run();

